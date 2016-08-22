open System
open System.IO
open System.Diagnostics

Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

[<AutoOpen>]
module Utils =
    let quote = sprintf "\"%s\""

    type Result = 
    | Success of TimeSpan 
    | Error of string * TimeSpan
    | Timeout

[<AutoOpen>]
module WebSharperCLI =
    type Arguments = Arguments of Argument list
        with
            override x.ToString() =
                match x with Arguments args -> args |> List.map string |> String.concat " "
    
    and Argument =
        | Bundle
        | OutputFile of string
        | OutputFolder of string
        | Reference of string
        | References of string seq
        with
            override x.ToString() =
                match x with
                | Bundle                -> "bundle"
                | OutputFile name       -> "-name " + name
                | OutputFolder folder   -> "-out " + folder
                | Reference reference   -> quote reference
                | References references -> references |> Seq.map quote |> String.concat " "

    let path =
        @".\packages\WebSharper.3.6.15.238\tools\net40\WebSharper.exe" |> quote
    
    let references = 
        [ @".\packages\WebSharper.3.6.15.238\lib\net40\WebSharper.Core.JavaScript.dll"
          @".\packages\WebSharper.3.6.15.238\lib\net40\WebSharper.Collections.dll"
          @".\packages\WebSharper.3.6.15.238\lib\net40\WebSharper.Core.dll"
          @".\packages\WebSharper.3.6.15.238\lib\net40\WebSharper.JavaScript.dll"
          @".\packages\WebSharper.3.6.15.238\lib\net40\WebSharper.JQuery.dll"
          @".\packages\WebSharper.3.6.15.238\lib\net40\WebSharper.Main.dll"
          @".\packages\WebSharper.3.6.15.238\lib\net40\WebSharper.Sitelets.dll"
          @".\packages\WebSharper.3.6.15.238\lib\net40\WebSharper.Web.dll"
          @".\packages\WebSharper.UI.Next.3.6.15.211\lib\net40\WebSharper.UI.Next.dll"
          @".\packages\WebSharper.UI.Next.3.6.15.211\lib\net40\WebSharper.UI.Next.Templating.dll" ]
    

(**
    Runs WebSharperCLI tool to transpile F# to JS using the "bundle" option.
    WebSharper.exe bundle -out [folder] -name [name] [references]
**)
module Program =
    let outputFolderName = "./SPA/Content"
    let outputJSFileName = "app"
    let dllPath          = "./SPA/bin/SPA.dll"
    let timeout          = TimeSpan.FromMilliseconds(10000.)

    match fsi.CommandLineArgs with
    | [| _; customer |] ->
        
        stdout.WriteLine "Start bundling..."

        let references = 
            Directory.GetFiles(Path.Combine(".", "configs", customer, "bin", "Debug"), "*.dll")
        
        let perf = Stopwatch.StartNew()
        let result =
            try

                // The order of argument is important here!
                // bundle 
                // -out [folder] 
                // -name [name] 
                // [references]
                let args =
                    Arguments [ Bundle
                                OutputFolder outputFolderName
                                OutputFile outputJSFileName
                                Reference dllPath
                                References references
                                References WebSharperCLI.references ]
                
                let processInfo = 
                        new ProcessStartInfo(
                            WebSharperCLI.path, 
                            string args, 
                            WindowStyle = ProcessWindowStyle.Hidden)
                
                use proc = Process.Start(processInfo)
                try
                    match proc.WaitForExit (int timeout.TotalMilliseconds) with
                    | true when proc.ExitCode = 0 ->
                        perf.Stop()
                        Success perf.Elapsed
    
                    | true -> 
                        perf.Stop()
                        Error (sprintf "Exited with error code %i." proc.ExitCode, perf.Elapsed)
                    
                    | false -> 
                        perf.Stop()
                        Timeout
                
                finally
                    try proc.Kill () with | _ -> ()

            with ex -> 
                perf.Stop()
                Error (ex.Message, perf.Elapsed)

        match result with
        | Success time ->      stdout.WriteLine(sprintf "Successfully transpiled to JS. [%fs]" time.TotalSeconds)
        | Error (msg, time) -> stderr.WriteLine(sprintf "%s [%fs]" msg time.TotalSeconds)
        | Timeout ->           stderr.WriteLine(sprintf "Process timed out. [%fs]" timeout.TotalSeconds)
    
    | _ as args ->
        stderr.WriteLine(sprintf "Couldn't parse arguments %A" args)

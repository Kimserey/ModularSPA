namespace Customer

open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client
open Modules

[<JavaScript>]
module Page =
    
    let doc = 
        [ ModuleC.page ]
        |> Seq.cast
        |> Doc.Concat
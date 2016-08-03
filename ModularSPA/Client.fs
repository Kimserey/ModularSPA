namespace ModularSPA

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Client

[<JavaScript>]
module Client =    
    let Main =
        [
            FeatureOne.doc
            FeatureTwo.doc
        ]
        |> Seq.cast
        |> Doc.Concat
        |> Doc.RunById "main"

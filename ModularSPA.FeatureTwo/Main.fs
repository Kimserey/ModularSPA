namespace ModularSPA

open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Html

[<JavaScript>]
module FeatureTwo =

    let doc =
        h1 [ text "This is feature TWO" ]
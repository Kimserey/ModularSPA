namespace ModularSPA

open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Html

[<JavaScript>]
module FeatureOne =

    let doc =
        h1 [ text "This is feature ONE" ]
namespace Modules

open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client

[<JavaScript>]
module ModuleC =

    let page =
        h1 [ text "This is module C" ]
namespace Modules

open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client

[<JavaScript>]
module ModuleA =

    let page =
        h1Attr [ attr.``class`` "test" ] [ text "This is module A" ]
namespace SPA

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client

[<JavaScript>]
module Client =    

    [<Inline """Customer.Page.doc()""">]
    let pages = X<Doc>

    let Main =
        pages
        |> Doc.RunById "main"

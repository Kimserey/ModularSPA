namespace ModularSPA

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Client

[<JavaScript>]
module Client =    
    
    [<Inline """ModularSPA.Configurations.pages()""">]
    let pages = X<Doc list>

    let Main =
        pages
        |> Doc.Concat
        |> Doc.RunById "main"

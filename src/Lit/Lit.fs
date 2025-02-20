namespace Lit

open System
open Browser.Types
open Fable.Core

[<AutoOpen>]
module DomHelpers =
    /// Wrapper for event handlers to help type checking.
    let inline Ev (handler: Event -> unit) = handler

    /// Wrapper for event handlers to help type checking.
    /// Extracts `event.target.value` and passes it to the handler.
    let inline EvVal (handler: string -> unit) = handler

    type Browser.Types.EventTarget with
        /// Casts the event target to HTMLInputElement and gets the `value` property.
        member this.Value = (this :?> Browser.Types.HTMLInputElement).value

        /// Casts the event target to HTMLInputElement and gets the `checked` property.
        member this.Checked = (this :?> Browser.Types.HTMLInputElement).``checked``

/// <summary>
/// The return type of the template tag functions.
/// </summary>
type TemplateResult =
    interface
    end

type RefValue<'T> =
    abstract value : 'T with get, set

    // /// Alias of `value` for compatibility with React's refs
    // [<Emit("$0.value")>]
    // abstract current : 'T with get, set

type RefValue = RefValue<obj>

/// <summary>
/// Base class for creating custom directives.
/// Users should extend this class, implement render and/or update,
/// and then pass their subclass to directive.
/// </summary>
[<ImportMember("lit-html/directive.js")>]
type Directive() =
    class
    end

/// <summary>
/// An abstract Directive base class whose disconnected method will be called
/// when the part containing the directive is cleared as a result of re-rendering,
/// or when the user calls part.setDirectiveConnection(false)
/// on a part that was previously rendered containing the directive.
/// </summary>
[<ImportMember("lit-html/async-directive.js")>]
type AsyncDirective() =
    member _.isConnected: bool = jsNative
    member _.setValue(value: obj) : unit = jsNative

[<Import("LitElement", "lit")>]
type LitElementBase() =
    member _.isConnected: bool = jsNative
    member _.connectedCallback(): unit = jsNative
    member _.disconnectedCallback(): unit = jsNative

type Part =
    interface
    end

type ChildPart =
    inherit Part
    abstract parentNode : Element

type ElementPart =
    inherit Part
    abstract element : Element

type Styles =
    interface end

type LitBindings =
    /// <summary>
    /// Interprets a template literal as an HTML template that can efficiently render to and update a container.
    /// </summary>
    [<ImportMember("lit-html")>]
    static member html: Template.JsTag<TemplateResult> = jsNative

    /// <summary>
    /// Interprets a template literal as an SVG template that can efficiently render to and update a container.
    /// </summary>
    [<ImportMember("lit-html")>]
    static member svg: Template.JsTag<TemplateResult> = jsNative

    [<ImportMember("lit")>]
    static member css: Template.JsTag<Styles> = jsNative

    /// <summary>
    /// Renders a value, usually a lit-html TemplateResult, to the container.
    /// </summary>
    /// <param name="el">The container to render into.</param>
    /// <param name="t">A <see cref="Lit.TemplateResult">TemplateResult</see> to be rendered.</param>
    [<ImportMember("lit-html")>]
    static member render(t: TemplateResult, el: Element) : unit = jsNative

    /// <summary>
    /// A sentinel value that signals a ChildPart to fully clear its content.
    /// </summary>
    [<ImportMember("lit-html")>]
    static member nothing: TemplateResult = jsNative

    /// <summary>
    /// A sentinel value that signals a ChildPart to fully clear its content.
    /// </summary>
    [<ImportMember("lit-html")>]
    static member noChange: TemplateResult = jsNative

    /// <summary>
    /// Creates a user-facing directive function from a Directive class.
    /// This function has the same parameters as the directive's render() method.
    /// </summary>
    [<ImportMember("lit-html/directive.js")>]
    static member directive(cons: obj) : obj = jsNative

    /// <summary>
    /// A directive that applies CSS properties to an element.
    /// </summary>
    [<ImportMember("lit-html/directives/style-map.js")>]
    static member styleMap(styles: obj) : obj = jsNative

    /// <summary>
    /// A directive that applies dynamic CSS classes.
    /// </summary>
    [<ImportMember("lit-html/directives/class-map.js")>]
    static member classMap(classes: obj) : obj = jsNative

    /// <summary>
    /// Renders one of a series of values, including Promises, to a Part.
    /// </summary>
    [<ImportMember("lit-html/directives/until.js")>]
    static member until([<ParamArray>] values: obj []) : TemplateResult = jsNative

    /// <summary>
    /// A directive that repeats a series of values (usually TemplateResults) generated from an iterable,
    /// and updates those items efficiently when the iterable changes based on user-provided keys associated with each item.
    /// </summary>
    /// <remarks>
    /// Important: keys must be unique for all items in a given call to repeat.
    /// The behavior when two or more items have the same key is undefined.
    /// </remarks>
    /// <param name="items">An sequence of items to be repeated.</param>
    /// <param name="getId">A function that maps an item in the sequence to a unique string key.</param>
    /// <param name="template">A template that will be rendered for each item in the iterable.</param>
    [<ImportMember("lit-html/directives/repeat.js")>]
    static member repeat<'T>(items: 'T seq, getId: 'T -> string, template: 'T -> int -> TemplateResult) = jsNative

    /// <summary>
    /// Enables fast switching between multiple templates by caching the DOM nodes and TemplateInstances produced by the templates.
    /// </summary>
    /// <param name="template">A template to be rendered.</param>
    [<ImportMember("lit-html/directives/cache.js")>]
    static member cache(template: TemplateResult) : TemplateResult = jsNative

    /// <summary>
    /// Prevents re-render of a template function until a single value or an array of values changes.
    /// </summary>
    /// <param name="deps">A set of dependencies that will be trigger a re-render when any of them changes.</param>
    /// <param name="render">A render function.</param>
    [<ImportMember("lit-html/directives/guard.js")>]
    static member guard(deps: obj array, render: unit -> TemplateResult) : TemplateResult = jsNative

    /// <summary>
    /// For AttributeParts, sets the attribute if the value is defined and removes the attribute if the value is undefined.
    /// </summary>
    /// <remarks>
    /// For other part types, this directive is a no-op.
    /// </remarks>
    /// <param name="value">A value to set the attribute to, or undefined to remove the attribute.</param>
    [<ImportMember("lit-html/directives/if-defined.js")>]
    static member ifDefined(value: obj) : TemplateResult = jsNative

    /// <summary>
    /// Sets the value of a Ref object or calls a ref callback with the element it's bound to.
    /// </summary>
    [<ImportMember("lit-html/directives/ref.js")>]
    static member ref(refOrCallback: obj) : TemplateResult = jsNative

    /// <summary>
    /// Creates a new Ref object, which is container for a reference to an element.
    /// </summary>
    [<ImportMember("lit-html/directives/ref.js")>]
    static member createRef<'T>() : RefValue<'T> = jsNative

[<AutoOpen>]
module LitHelpers =
    /// <summary>
    /// Interprets a template literal as an HTML template that can efficiently render to and update a container.
    /// </summary>
    let html: Template.Tag<_> = Template.transform LitBindings.html

    /// <summary>
    /// Interprets a template literal as an SVG template that can efficiently render to and update a container.
    /// svg is required for nested templates within an svg element
    /// </summary>
    let svg: Template.Tag<_> = Template.transform LitBindings.svg

    /// CSS used in the Shadow DOM of LitElements
    let css: Template.Tag<_> = Template.transform LitBindings.css

    /// Just trims the braces {} out of a css block to be used in a `style` attribute
    let inline_css (css: string) =
        match css.IndexOf("{") with
        | -1 -> css
        | i ->
            match css.LastIndexOf("}") with
            | i2 when i2 > i -> css.[i+1..i2-1]
            | _ -> css

type Lit() =
    /// <summary>
    /// Interprets a template literal as an HTML template that can efficiently render to and update a container.
    /// </summary>
    static member html = html

    /// <summary>
    /// Interprets a template literal as an SVG template that can efficiently render to and update a container.
    /// svg is required for nested templates within an svg element
    /// </summary>
    static member svg = svg

    /// CSS used in the Shadow DOM of LitElements
    static member css = css

    /// <summary>
    /// A sentinel value that signals a ChildPart to fully clear its content.
    /// </summary>
    static member nothing = LitBindings.nothing

    /// <summary>
    /// Renders a value, usually a lit-html TemplateResult, to the container.
    /// </summary>
    /// <param name="el">The container to render into.</param>
    /// <param name="t">A <see cref="Lit.TemplateResult">TemplateResult</see> to be rendered.</param>
    static member render el t = LitBindings.render (t, el)

    /// <summary>
    /// Generates a single string that filters out false-y values from a tuple sequence.
    /// </summary>
    static member classes(classes: (string * bool) seq) =
        classes
        |> Seq.choose (fun (s, b) -> if b then Some s else None)
        |> String.concat " "

    /// <summary>
    /// Generates a string from the string seuence provided
    /// </summary>
    static member classes(classes: string seq) = classes |> String.concat " "

    /// <summary>
    /// Use memoize only when the template argument can change considerable depending on some condition (e.g. a modal or nothing).
    /// </summary>
    /// <param name="template">A template to be rendered.</param>
    static member memoize(template: TemplateResult) : TemplateResult = LitBindings.cache template

    static member ofSeq(items: TemplateResult seq) : TemplateResult = unbox items

    static member ofList(items: TemplateResult list) : TemplateResult = unbox items

    /// <summary>
    /// Give a unique id to items in a list. This can improve performance in lists that will be sorted, filtered or re-ordered.
    /// </summary>
    /// <param name="getId">A function that maps an item in the sequence to a unique string key.</param>
    /// <param name="template">A rendering function based on the items of the sequence.</param>
    /// <param name="items">A sequence of items to be rendered.</param>
    static member mapUnique (getId: 'T -> string) (template: 'T -> TemplateResult) (items: 'T seq) =
        LitBindings.repeat (items, getId, (fun x _ -> template x))

    /// <summary>
    /// Prevents re-render of a template function until a single value or an array of values changes.
    /// </summary>
    /// <param name="dependencies">A set of dependencies that will be trigger a re-render when any of them changes.</param>
    /// <param name="view">A render function.</param>
    static member ofLazy (dependencies: obj list) (view: unit -> TemplateResult) : TemplateResult =
        // TODO: Should we try to use F# equality here?
        LitBindings.guard (List.toArray dependencies, view)

    /// <summary>
    /// Shows the placeholder until the promise is resolved
    /// </summary>
    /// <param name="deferred">A promise to be resolved.</param>
    /// <param name="placeholder">A placeholder to be shown while the promise is pending.</param>
    static member ofPromise (placeholder: TemplateResult) (deferred: JS.Promise<TemplateResult>) =
        LitBindings.until (deferred, placeholder)

    static member ofStr(v: string) : TemplateResult = unbox v

    static member ofText(v: string) : TemplateResult = unbox v

    static member ofInt(v: int) : TemplateResult = unbox v

    static member ofFloat(v: float) : TemplateResult = unbox v

    /// <summary>
    /// Sets the attribute if the value is defined and removes the attribute if the value is undefined.
    /// </summary>
    /// <param name="attributeValue">A value to set the attribute to</param>
    static member attrOfOption(attributeValue: string option) = LitBindings.ifDefined attributeValue

    /// Creates an optional ref with no content. The ref can be used with Lit.refValue or Lit.refFn to get a reference of a DOM element.
    static member createRef<'T>() : RefValue<'T option> = LitBindings.createRef<'T option> ()

    /// Creates a ref with the given value. The ref can be used with Lit.refValue or Lit.refFn to get a reference of a DOM element.
    static member createRef(value: 'T) : RefValue<'T> =
        let r = LitBindings.createRef<'T> ()
        r.value <- value
        r

    /// When placed on an element in the template, the ref directive will retrieve a reference to that element once rendered.
    /// Example: <input {Lit.refValue inputRef}>
    static member refValue<'El when 'El :> Element>(v: RefValue<'El option>) = LitBindings.ref v

    /// When placed on an element in the template, the callback will be called each time the referenced element changes.
    /// If a ref callback is rendered to a different element position or is removed in a subsequent render,
    /// it will first be called with undefined, followed by another call with the new element it was rendered to (if any).
    /// Example: <input {Lit.refFn inputFn}>
    static member refFn<'El when 'El :> Element>(fn: 'El option -> unit) = LitBindings.ref fn

    /// Used when building custom directives. [More info](https://lit.dev/docs/templates/custom-directives/).
    static member inline directive<'Class, 'Arg>() : 'Arg -> TemplateResult =
        LitBindings.directive JsInterop.jsConstructor<'Class> :?> _

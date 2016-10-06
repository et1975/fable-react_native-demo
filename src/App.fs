module internal Nightwatch

open System
open Fable.Core
open Fable.Import
open Fable.Import.ReactNative
open Fable.Helpers.ReactNative
open Fable.Helpers.ReactNative.Props
open Elmish
open Types.Nightwatch
open State.Nightwatch
open Scenes

let view (model:AppModel) (dispatch: AppMsg -> unit) =
    match model.NavigationStack with
    | [] -> Home.view model.HomeModel (HomeMsg >> dispatch)
    | Page.Home::_ -> Home.view model.HomeModel (HomeMsg >> dispatch)
    | Page.CheckLocation(i)::_ -> LocationCheck.view model.CheckLocationModel (LocationCheckMsg >> dispatch)
    | Page.LocationList::_ -> Locations.view model.LocationListModel (LocationListMsg >> dispatch)

let loaded = ref false

// App
let program =
    Program.mkProgram init update
//    |> Program.withSubscription subscribe
    |> Program.withConsoleTrace
    
type Nightwatch (props) as this =
    inherit React.Component<obj,AppModel>(props)

    let safeState state = 
        match !loaded with 
        | false -> this.state <- state
        | _ -> this.setState state

    let dispatch = program |> Program.run safeState 
    let backHandler = (fun () -> dispatch AppMsg.NavigateBack; true)
        
    member x.componentDidMount() =
        if not !loaded then
            Fable.Helpers.ReactNative.setOnHardwareBackPressHandler backHandler

        loaded := true

    member x.componentWillUnmount() =  
        if !loaded then
            Fable.Helpers.ReactNative.removeOnHardwareBackPressHandler backHandler

        loaded := false

    member x.render () =
        view this.state dispatch

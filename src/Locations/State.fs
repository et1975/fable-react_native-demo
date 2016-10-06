module internal State.Locations

open System
open Fable.Core
open Fable.Import
open Fable.Import.ReactNative
open Fable.Import.ReactNativeImagePicker
open Fable.Helpers.Fetch
open Fable.Helpers.ReactNative
open Fable.Helpers.ReactNative.Props
open Fable.Helpers.ReactNativeSimpleStore
open Fable.Core.JsInterop
open Elmish
open Types.LocationCheck
open Types.Locations

let getDemoData() =
    async { 
        let! requests =
            fetchAs<Types.LocationCheck.Model[]>
                ("https://raw.githubusercontent.com/fsprojects/fable-react_native-demo/master/demodata/LocationCheckRequests.json",
                [])
        do! DB.addMultiple requests
        return
            requests
            |> Array.mapi (fun i r -> i,r)
    } 


let getIndexedCheckRequests () =
    async {
        let! requests = DB.getAll<Types.LocationCheck.Model>()
        return 
            requests
            |> Array.mapi (fun i r -> i,r)
    }

let init () =
    { Status = NotStarted
      Current = None
      Locations = [] }, Cmd.ofAsync getDemoData () Msg.NewLocationCheckRequests Msg.Error


// Update
let update msg model : Model*Cmd<Msg> =
    match msg with
    | Msg.Completed ->
        model, []
    | Msg.CheckNextLocation pos ->
        { model with
            Current = Some (model.Locations |> List.item pos |> snd)
            Status = InProgress }, []
    | Msg.RefreshList ->
        { model with
            Status = InProgress }, Cmd.ofAsync getIndexedCheckRequests () Msg.NewLocationCheckRequests Msg.Error
    | Msg.NewLocationCheckRequests indexedRequests ->
        { model with
            Locations = model.Locations
            Status = Complete (sprintf "Locations: %d" indexedRequests.Length) }, []
    | Msg.Error e ->
        { model with
            Status = Complete (string e.Message) }, []
    | Msg.LocationCheckMsg subMsg ->
        let (m,cmd) = LocationCheck.update subMsg model.Current.Value
        { model with Current = Some m}, cmd |> Cmd.map Msg.LocationCheckMsg

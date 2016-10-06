module internal State.Home

open System
open Fable.Core
open Fable.Import
open Fable.Import.ReactNative
open Fable.Helpers.ReactNative
open Fable.Helpers.ReactNative.Props
open Elmish
open Types.Home


// Update
let update (msg:Msg) model : Model*Cmd<Msg> = 
    match msg with
    | Msg.NewDemoData count ->
        { model with StatusText = sprintf "Locations: %d" count }, []
    | Msg.BeginWatch ->
        { model with StatusText = "" }, []
    | Msg.Error e ->
        { model with StatusText = string e.Message }, []


let init () = { StatusText = "Syncing..." }, []

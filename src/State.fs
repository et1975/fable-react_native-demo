module internal State.Nightwatch

open System
open Fable.Core
open Fable.Import
open Fable.Import.ReactNative
open Fable.Helpers.ReactNative
open Fable.Helpers.ReactNative.Props
open Fable.Helpers.ReactNativeSimpleStore
open Fable.Import
open Fable.Import.Fetch
open Fable.Helpers.Fetch
open Elmish
open Types
open Types.Nightwatch
open State

let init() =
    let homeModel,homeCmd = Home.init() 
    let locationsModel,locationsCmd = Locations.init() 
    { HomeModel = homeModel
      LocationListModel = locationsModel
      NavigationStack = [Page.Home] }, Cmd.batch[ homeCmd |> Cmd.map HomeMsg
                                                  locationsCmd |> Cmd.map LocationListMsg ]

let navigateTo page newStack model =
    match page with
    | Page.LocationList -> Locations.init() |> wrap LocationListModel model
    | Page.CheckLocation (pos,request) -> CheckLocation.init(pos,request) |> wrap CheckLocationModel model
    | Page.Home -> Home.init() |> wrap HomeModel model
    |> fun (model,cmd) -> { model with NavigationStack = newStack },cmd

let update (msg:AppMsg) model : AppModel*Cmd<AppMsg> = 
    match msg with
    | HomeMsg subMsg ->
        let (m,cmd) = Home.update subMsg model.HomeModel
        let myCmd = 
            match subMsg with
            | Home.Msg.BeginWatch -> Cmd.ofMsg (AppMsg.NavigateTo Page.LocationList)
            | _ -> []
        { model with HomeModel = m}, Cmd.batch [myCmd
                                                cmd |> Cmd.map HomeMsg]

    | LocationListMsg subMsg ->
        let (m,cmd) = Locations.update Locations.Msg.Completed model.LocationListModel
        let myMsg = 
            match subMsg with
            | Locations.Msg.Completed -> Cmd.ofMsg NavigateBack
            | Locations.Msg.NewLocationCheckRequests rs -> Cmd.ofMsg (Home.Msg.NewDemoData rs.Length) |> Cmd.map HomeMsg
            | Locations.Msg.Error err -> Cmd.ofMsg (Home.Msg.Error err) |> Cmd.map HomeMsg
            | _ -> [] 
        { model with LocationListModel = m}, Cmd.batch [myMsg
                                                        cmd |> Cmd.map LocationListMsg]
    | NavigateTo page -> navigateTo page (page::model.NavigationStack) model

    | NavigateBack -> 
        match model.NavigationStack with
        | _::page::rest -> navigateTo page (page::rest) model
        | _ -> model,Cmd.ofMsg ExitApp

    | ExitApp -> 
        Fable.Helpers.ReactNative.exitApp() 
        model,Cmd.none

// let subscribe _ =
//     fun dispatch ->
//         Fable.Helpers.ReactNative.setOnHardwareBackPressHandler backHandler


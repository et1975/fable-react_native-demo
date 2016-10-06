module internal Types.Locations

open System
open Fable.Core
open Fable.Import
open Fable.Import.ReactNative
open Fable.Helpers.ReactNative
open Fable.Helpers.ReactNative.Props
open Elmish
open Types.LocationCheck

// Model
type Status =
| NotStarted
| InProgress
| Complete of string


type Model =
  { Locations: (int * LocationCheck.Model) list
    Current: LocationCheck.Model option
    Status : Status } 

type [<RequireQualifiedAccess>] Msg =
| CheckNextLocation of int
| RefreshList
| NewLocationCheckRequests of (int * LocationCheck.Model)[]
| Error of exn
| LocationCheckMsg of LocationCheck.Msg
| Completed

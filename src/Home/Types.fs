module internal Types.Home

open System
open Fable.Core
open Fable.Import
open Fable.Import.ReactNative
open Fable.Helpers.ReactNative
open Fable.Helpers.ReactNative.Props
open Elmish

// Model
type Model = { StatusText : string  }

type [<RequireQualifiedAccess>] Msg =
| NewDemoData of int
| BeginWatch
| Error of exn

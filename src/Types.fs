module internal Types.Nightwatch

open System

[<RequireQualifiedAccess>]
type Page =
| Home
| LocationList
| CheckLocation of int

type AppMsg =
| NavigateTo of Page
| NavigateBack
| ExitApp
| HomeMsg of Home.Msg
| LocationListMsg of Locations.Msg

type AppModel = {
    HomeModel: Home.Model
    LocationListModel: Locations.Model
    NavigationStack: Page list
}

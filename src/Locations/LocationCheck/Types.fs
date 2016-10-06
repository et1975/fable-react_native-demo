module internal Types.LocationCheck

open System

// Model
type Status =
| Unchanged
| Changed
| Error of string

type LocationId = string

[<RequireQualifiedAccess>]
type LocationStatus =
| Ok
| Alarm of string

type LocationCheckRequest = {
    LocationId : LocationId
    Name: string
    Address: string    
    Status : LocationStatus option
    Date : DateTime option
}

type Model =
  { LocationCheckRequest : LocationCheckRequest
    Position : int
    PictureUri : string option
    Status : Status } 

type [<RequireQualifiedAccess>] Msg =
| PictureSelected of string option
| LocationStatusUpdated of LocationStatus
| SelectPicture
| Save
| GoBack
| Error of exn


module internal State.LocationCheck

open System
open Fable.Core
open Fable.Import
open Fable.Import.ReactNative
open Fable.Import.ReactNativeImagePicker
open Fable.Helpers.ReactNative
open Fable.Helpers.ReactNative.Props
open Fable.Helpers.ReactNativeSimpleStore
open Fable.Helpers.ReactNativeImagePicker
open Fable.Helpers.ReactNativeImagePicker.Props
open Fable.Import.JS
open Elmish
open Types.LocationCheck

let init (pos,request) =
    { Status = Unchanged
      Position = pos
      PictureUri = None
      LocationCheckRequest = request }, Cmd.none

// Helpers update
let save (pos,request : LocationCheckRequest) = DB.update(pos,request) 

let selectImage () =
    showImagePickerAsync
        [Title "Take picture"
         AllowsEditing true]

// Update
let update (msg:Msg) model : Model*Cmd<Msg> =
    match msg with
    | Msg.GoBack ->
        model,[]
    | Msg.Save ->
        match model.Status with
        | Unchanged -> model,[]
        | _ -> model,Cmd.ofAsync save (model.Position,model.LocationCheckRequest) (fun _ -> Msg.GoBack) Msg.Error
    | Msg.PictureSelected selectedPicture ->
        { model with
            PictureUri = selectedPicture
            Status = Changed }, []
    | Msg.SelectPicture ->
        model,Cmd.ofAsync selectImage () Msg.PictureSelected Msg.Error // there might be a way to do this in the view, not state
    | Msg.LocationStatusUpdated newStatus ->
        { model with
            LocationCheckRequest = 
                { model.LocationCheckRequest with 
                      Status = Some newStatus
                      Date = Some DateTime.Now }
            Status = Changed }, []
    | Msg.Error e ->
        { model with
            Status = Error (string e.Message) }, []


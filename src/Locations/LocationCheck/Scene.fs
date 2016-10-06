module internal Scenes.LocationCheck

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

// View
let view (model:Model) (dispatch: Msg -> unit) =
    let selectImageButton =
        Styles.button "Take picture"
            (fun () -> dispatch Msg.SelectPicture)

    let image =
        match model.PictureUri with
        | Some uri -> 
            image
                [ Source [ Uri uri; IsStatic true]
                  ImageProperties.Style [
                    ImageStyle.BorderColor "#000000"
                    ImageStyle.Flex 3
                  ]
                ]                
        | None -> 
            image
                [ Source (localImage "../../images/snow.jpg")
                  ImageProperties.Style [
                    ImageStyle.BorderColor "#000000"
                    ImageStyle.Flex 3
                    ImageStyle.AlignSelf Alignment.Center
                  ]
            ]

    view [ Styles.sceneBackground ] 
        [ text [ Styles.defaultText ] model.LocationCheckRequest.Name
          textInput [
            TextInputProperties.AutoCorrect false
            TextInputProperties.Style [
                TextStyle.MarginTop 2.
                TextStyle.MarginBottom 2.
                TextStyle.Color Styles.textColor
                TextStyle.BackgroundColor Styles.inputBackgroundColor
              ]
            TextInputProperties.OnChangeText (LocationStatus.Alarm >> Msg.LocationStatusUpdated >> dispatch)
          ] ""

          image
          selectImageButton
          view 
            [ ViewProperties.Style [
                ViewStyle.JustifyContent JustifyContent.Center
                ViewStyle.AlignItems ItemAlignment.Center
                ViewStyle.Flex 1
                ViewStyle.FlexDirection FlexDirection.Row ]]
              [ Styles.verticalButton "Cancel" (fun () -> dispatch Msg.GoBack)
                Styles.verticalButton "OK" (fun () -> dispatch (Msg.Save)) ]
        ]
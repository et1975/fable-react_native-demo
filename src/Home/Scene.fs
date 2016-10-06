module internal Scenes.Home

open System
open Fable.Core
open Fable.Import
open Fable.Import.ReactNative
open Fable.Helpers.ReactNative
open Fable.Helpers.ReactNative.Props
open Elmish
open Types.Home

// View 
let view (model:Model) (dispatch:Dispatch<Msg>) =
      let logo =
          image 
              [ Source (localImage "../../images/raven.jpg")
                ImageProperties.Style [
                  ImageStyle.AlignSelf Alignment.Center
                  ImageStyle.FlexDirection FlexDirection.Column
                ]
              ]

      view [ Styles.sceneBackground ] 
        [ text [ Styles.titleText ] "Nightwatch"
          logo
          Styles.button "Begin watch" (fun () -> dispatch Msg.BeginWatch)
          Styles.whitespace
          Styles.whitespace
          text [ Styles.smallText ] model.StatusText  ]

module internal Scenes.Locations

open System
open Fable.Core
open Fable.Import
open Fable.Import.ReactNative
open Fable.Import.ReactNativeImagePicker
open Fable.Helpers.ReactNative
open Fable.Helpers.ReactNative.Props
open Fable.Core.JsInterop
open Elmish
open Types.Locations

let viewDataSource: ListViewDataSource<int * Types.LocationCheck.Model> = emptyDataSource()

// View
let view (model:Model) (dispatch: Dispatch<Msg>) =
    let getListView() =
        let ds = updateDataSource (model.Locations |> Array.ofList) viewDataSource
        listView 
            ds
            [ ListViewProperties.RenderRow  
                (Func<_,_,_,_,_>(fun (pos,request) b c d ->
                    view [
                        ViewProperties.Style[
                            ViewStyle.JustifyContent JustifyContent.Center
                            ViewStyle.AlignItems ItemAlignment.Center
                            ViewStyle.Flex 1
                            ViewStyle.FlexDirection FlexDirection.Row ]]
                        [ text [ Styles.defaultText ] request.LocationCheckRequest.Name
                          text [ Styles.defaultText ] request.LocationCheckRequest.Address
                          (match request.LocationCheckRequest.Status with
                           | None -> text [] ""
                           | Some status ->
                                let uri = 
                                    match status with
                                    | Types.LocationCheck.LocationStatus.Alarm text -> localImage "../../images/Alarm.png"
                                    | _ -> localImage "../../images/Approve.png"

                                image
                                    [ Source uri
                                      ImageProperties.Style [
                                            ImageStyle.Width 24.
                                            ImageStyle.Height 24.
                                            ImageStyle.AlignSelf Alignment.Center
                                        ]
                                    ])
                         ]
                    |> touchableHighlight [OnPress (fun () -> dispatch (Msg.CheckNextLocation pos))]))
            ]

    view [ Styles.sceneBackground ] 
        [ text [ Styles.titleText ] "Locations to check"
          text [ Styles.defaultText ] 
            (match model.Status with 
             | Complete s -> s
             | _ -> "")
          (if model.Locations.Length > 0 then getListView() else Styles.whitespace)

          Styles.button "OK" (fun () -> dispatch Msg.Completed)
        ]
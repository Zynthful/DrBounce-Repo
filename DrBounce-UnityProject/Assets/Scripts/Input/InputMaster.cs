// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""9185f850-f99d-4d68-8623-95a58b8140fb"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""fa96ebe6-69ea-4988-887e-88fbc3a1f730"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""cd1e5fcd-e0ba-4129-97a8-ae44cd6001b4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""Value"",
                    ""id"": ""461042ae-515d-4692-b304-916296ec86e1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""6145e5cb-193b-42de-a7a3-b808f5915652"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Recall"",
                    ""type"": ""Button"",
                    ""id"": ""c59cad59-5098-482b-b35f-e45d94edc138"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Throw"",
                    ""type"": ""Button"",
                    ""id"": ""c7f9b13c-02ba-4148-8fc0-cb0bfadead58"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0de4bb50-dd38-4346-8e6d-db9b81f0c5e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.01)""
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""08741584-245b-4eae-b1c1-0eb5c17aeb6e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim Assist"",
                    ""type"": ""Button"",
                    ""id"": ""0a72784a-10fc-419b-89f0-c16208e7a319"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Switch Held"",
                    ""type"": ""Button"",
                    ""id"": ""8c878912-46d1-4610-a559-c850d0f704bd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Heal"",
                    ""type"": ""Button"",
                    ""id"": ""899195a3-ba67-4997-9d09-8f535e64f3c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Toggle Debug Controls"",
                    ""type"": ""Button"",
                    ""id"": ""dba99ac6-5cd2-4a4d-bf73-e626bb152037"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ac3ad8f4-b1a4-4241-874d-fdce0e3b03f4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4fa54fc0-360e-45ac-b14f-ca2f3d56bfb0"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15a240ca-1219-4511-8d0f-728847538101"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e516c791-0e62-49e6-8958-cc81a8c403c3"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""f270b81c-73d0-403e-8a1c-f73b358fbd8c"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""48046f56-6e4c-4765-8b53-4a7375afe867"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a07ad1cf-692f-4b0e-86e4-33481f60c212"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4938f88b-5fef-4b4b-bf8f-342b23ae7628"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9d53201c-c31d-4a69-9cda-da404b5ec503"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e0e13786-3fe7-439a-852f-ba467354d245"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d0f8c9d6-b4dc-434e-b829-6cbed3d99c1d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9f756d1a-c4f3-42eb-990d-135e9454a89c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b483646a-1c36-40ec-a7d2-fcb961716530"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""20c862c3-3000-47f3-9dee-8659022f2f76"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""b9c07270-bfa8-4a6d-b5cc-05cba43f343b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4237f94f-7a18-4c5d-ac60-7c1a2f8fc749"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9c11cb0f-ccec-497a-9de4-193394a862bc"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8fa82dc7-6661-429f-9e71-34e89b8384c3"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6c9aab11-efb6-4510-94c4-e48c85acc1cf"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""ad56a1a6-78f2-4ff4-8bbc-29afc3cb0d05"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""9c6b7a76-481e-462a-9d27-bbb6379fadf5"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""44c17118-f4a0-4af4-8861-39a84b7a09e8"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""ac3ab63a-c89f-4cb8-a0ee-90f0e8fb1d31"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""bd510920-3c70-48a9-8aa1-ed729fe4528a"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""bfe848f7-b9cd-4a3d-9aaf-4c0d980c8f66"",
                    ""path"": ""<Gamepad>/rightStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ace3c805-db45-47aa-9cda-329ebc160e7d"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82f141fa-cf7b-4175-a071-5a1eea971f2a"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e4fc9a9-dd26-42ba-bcf9-658db53949f3"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01bf4de3-593a-4a1f-8f3c-48e2840bde58"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4a9b64a-59f9-492d-8c4d-8abd07ef0e2e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""88a64063-c7d1-4f98-aa13-3ddbe4a25696"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d47a0d0f-4cc1-4f62-a0fc-b410e1bcc62f"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6c68582-7840-483a-a4fc-e15451c5692c"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff54b5f9-2cd5-4fb7-82f8-988a4806ad6b"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Recall"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5e85bd5-194d-415b-8037-49a879905183"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Recall"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ffa1bbb-9d0e-4fd2-8846-2809bd736077"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Recall"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c6ba491e-9851-4d2f-96ca-60efe5360542"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Recall"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0fde9cbd-815d-441d-9ef4-74b20c5f8869"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f7363aa-6d73-485e-98dc-e78a80ea332a"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c86b167-f9a2-4836-b6eb-59e22a1c0f10"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dfacb740-7e11-437b-b841-3a755194a958"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f145941f-8093-456b-b604-4c866a5a7563"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc9b3062-a479-4008-a9b0-04722fb75f80"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ca9b3fb-6ba4-4366-9ab0-69e95a140915"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8239fa6-de8b-44f1-9454-f5bb1c02db95"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a3d0f819-f76e-4d42-a5bc-116c7c61159e"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Aim Assist"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7c81cf1-4b0f-4122-a7a6-5d96ce29eeca"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Aim Assist"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19b5f60f-ba86-4241-a258-aebc6468e5c9"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aim Assist"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5bd67e8c-34fb-401f-a896-baf706ee5756"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aim Assist"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""239d98c4-2921-4f9a-92a6-8215966fa426"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Switch Held"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb5e64ad-34d2-45c7-a149-d2c46fb5bee1"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Switch Held"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87452c7f-7697-4d8f-9e14-12b9f07f202c"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Switch Held"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e31e413f-d777-4790-b258-3d4c19843e00"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Switch Held"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6be69ae5-7227-443a-992d-4352e7f20e1b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Heal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d0c479ca-f7c5-4c10-9f77-bc2a877b9b72"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Heal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81591703-7841-4d3b-8030-8ec094ae1c80"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Heal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1fb3f013-82d2-4330-a55e-bcc1e4543e37"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Heal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7ff3773-5fe7-43f6-86a3-bb06e85f4a30"",
                    ""path"": ""<Keyboard>/semicolon"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Toggle Debug Controls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5b2a329-6083-4153-a9ca-07dda359a258"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Toggle Debug Controls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""453c55b2-3880-4c80-91b2-ac8bdd99fa63"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Toggle Debug Controls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""875f5c64-d8d3-4f95-9177-6ee9f913f871"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Toggle Debug Controls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""dd374058-9b07-44a5-9ebc-0db1169c32e4"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""28685e3a-3b2e-4a8e-b3b9-c3829b63052a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Continue"",
                    ""type"": ""Button"",
                    ""id"": ""80ce1b7b-3fce-4595-b9af-ae7bba271b21"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Next"",
                    ""type"": ""Button"",
                    ""id"": ""65eaae44-bc23-4136-966e-23f86e5c5ece"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reset All"",
                    ""type"": ""Button"",
                    ""id"": ""97727f55-d72a-4d18-ac4f-421eb6e33fce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Next (Sub Tab)"",
                    ""type"": ""Button"",
                    ""id"": ""4bfed5c6-139e-4e25-aa0c-225b741528d6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Previous"",
                    ""type"": ""Button"",
                    ""id"": ""0140d14a-8ec2-4737-a741-37d22d6c063f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Previous (Sub Tab)"",
                    ""type"": ""Button"",
                    ""id"": ""e00f4a2c-268d-4ed7-9ec9-b6c4c278d28c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""df6187da-1734-43d9-a826-2df3c5d3d8b5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""18453149-57f0-4ce8-808e-4f8f00edeaab"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e63d21fd-335c-449b-bc4f-17aeef46e212"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""209c68cf-db40-4eeb-9a21-7fa834cf1116"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0dc8a61-3a7b-453b-bce8-92cb5b31a0e3"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c2bcf952-4677-44ad-9253-78915fb66d7d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34707ae8-d614-4e5f-b272-b03699283f78"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8b385402-ca43-45c0-9ba9-673f69a74662"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e847bdbf-b8fa-4d38-a8e1-cbb660220b5a"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad34cd5e-889a-485b-a257-066db3cbfc39"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""208692bd-7aa4-4e1c-a725-0a9c7e56bf8c"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b70b55b6-ecaa-423a-b1d7-bf3dd9e8414c"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c73fd05d-586a-4665-9d98-56d55412424b"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4590337-ca5b-4886-90d6-3d6d0f2f3d20"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Previous"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24eb8c38-c345-43b5-ac32-5093e27a6a30"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Previous"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2d37ea3d-a70c-4e67-9dcf-ce59c814c374"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Previous"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96d2f91e-db2c-48ba-a414-ce79943b227d"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Previous"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7123851-a0f6-4e64-9311-ea1a54d4e9c2"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""73bafa72-c4b3-490a-a4dc-787f4ef1fb03"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d83f03cb-0a77-44b1-8625-e56c378a7867"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fabc3998-edd8-4d18-b318-282c7aec9b52"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82e9040f-d517-478e-8b4f-9d36368f7617"",
                    ""path"": """",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Next (Sub Tab)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41fe3cda-dfda-4939-92a4-208a1d6d7ca8"",
                    ""path"": """",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Next (Sub Tab)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a3f7ce4-d795-4d8a-aed1-6b3131aa841f"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Next (Sub Tab)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b4d427f-2610-4914-ac38-6019ada5675a"",
                    ""path"": """",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Next (Sub Tab)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01f52bbc-757b-4d41-9590-45cc15ce1b88"",
                    ""path"": """",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Previous (Sub Tab)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""478c3aa2-10af-449d-b482-0dbca6bcbfc2"",
                    ""path"": """",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Previous (Sub Tab)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""915c5bea-1b55-4bea-80b7-107e72b9029d"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Previous (Sub Tab)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5494439e-0399-4ece-bcc7-0b2c647fcd7a"",
                    ""path"": """",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Previous (Sub Tab)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e9817b89-e398-423d-9efd-62b21733c0ba"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Reset All"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8669adfe-1276-4e4e-b67a-cc29ea99296b"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Reset All"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c0aaf6c-f22b-4c22-b6bc-f182d86aa3c3"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Reset All"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5947f8ac-467e-4dc1-98c9-49a4634744ed"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Reset All"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Cutscene"",
            ""id"": ""3595e960-3f68-4b6e-bc25-40de9e4fd298"",
            ""actions"": [
                {
                    ""name"": ""SkipCutscene"",
                    ""type"": ""Button"",
                    ""id"": ""c14458bb-3510-4d43-8f96-6537b3b3b1f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6963477f-00fc-46b4-afeb-f7f93e3d2ea1"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipCutscene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7736dad4-1b6b-4adb-a10a-45da4484d457"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipCutscene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38072035-a215-4399-882f-c702254efe7a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipCutscene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""055fd26d-e412-4853-9fa3-4d57473ecf73"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipCutscene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""88ca4da0-01b5-4cb8-a584-9f0baf761db8"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipCutscene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""68d4e04c-9958-4f38-9810-b47193fbe0a8"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipCutscene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b271a1fe-2f9e-4b4c-b0e2-764d2cb12d4e"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipCutscene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Debug"",
            ""id"": ""0bb7150d-53a8-48e7-8b1e-e5f6a2aeda09"",
            ""actions"": [
                {
                    ""name"": ""DEBUG_PrevLevel"",
                    ""type"": ""Button"",
                    ""id"": ""c77bc23e-8e8c-4643-b53c-11c3a12acf08"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DEBUG_NextLevel"",
                    ""type"": ""Button"",
                    ""id"": ""a8b8775c-d223-4b0d-8f8b-4ef0840f92fc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DEBUG_Pause"",
                    ""type"": ""Button"",
                    ""id"": ""f2e03031-1bf9-4965-a80f-4a886929cc60"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DEBUG_ReloadScene"",
                    ""type"": ""Button"",
                    ""id"": ""e8737602-6d0b-4cb5-8884-74f917cfaeed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DEBUG_NextCheckpoint"",
                    ""type"": ""Button"",
                    ""id"": ""18f47463-10fc-4cd4-8c72-22c121dca6db"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DEBUG_PrevCheckpoint"",
                    ""type"": ""Button"",
                    ""id"": ""7025722a-9d29-4e32-b0f3-34b90730dc73"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""70244c03-3e90-4441-baaa-ab9ba5158832"",
                    ""path"": ""<Keyboard>/leftBracket"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""DEBUG_PrevLevel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5a620a5-0622-46c9-8987-daa37d8104e3"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""DEBUG_PrevLevel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""712743e3-cb4e-481d-a925-4cb102f41b87"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DEBUG_PrevLevel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""47df249d-bb0f-4aae-8772-37b559d061d8"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DEBUG_PrevLevel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""84ac7418-ef29-4ede-bc2c-ef87cb0e0a91"",
                    ""path"": ""<Keyboard>/rightBracket"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""DEBUG_NextLevel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""06562d2e-9347-4c03-9b0c-c43f160e20f2"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""DEBUG_NextLevel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""efded8fc-80ae-464e-a53c-5cedc0f5df90"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DEBUG_NextLevel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f4089e5-b99f-4e09-948f-d9dfdee57728"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DEBUG_NextLevel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83488a52-23e7-44ec-b75a-5b07e8a01a20"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""DEBUG_Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ee86101-7243-4ca5-9fc8-dd0fbc0c2e1e"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""DEBUG_Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d34643c3-6663-4527-abf8-4355e0bc8873"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DEBUG_Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7be7037a-4269-48b7-9740-efa118b6d917"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DEBUG_Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a9bf7e7-16e5-4a07-a403-bc50d1dd85b2"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""DEBUG_ReloadScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75529e49-9772-4940-a609-e8fbcdccdd5a"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""DEBUG_ReloadScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c032ae82-54a4-457d-9f4b-7d0b621a60ae"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DEBUG_ReloadScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6073b478-d30b-4d40-b502-c6d010dfe8e0"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DEBUG_ReloadScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fbccb605-271b-4a5a-9774-2b3d88a07f7e"",
                    ""path"": ""<Keyboard>/numpadPlus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""DEBUG_NextCheckpoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca0575ef-24ae-4dd4-b1e0-944563a66a8e"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""DEBUG_NextCheckpoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3029c0d0-40ed-422f-a44d-fce1e5cbbd89"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DEBUG_NextCheckpoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2809a33b-4d9f-45f4-ad6c-716d57425378"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DEBUG_NextCheckpoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""319ef88c-c78b-4b2d-b340-7d280c67631f"",
                    ""path"": ""<Keyboard>/numpadMinus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""DEBUG_PrevCheckpoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""daf4e6d0-0078-4eba-9d41-9cbf0c3376bc"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""DEBUG_PrevCheckpoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69bee84f-bffb-4349-8235-533aaac37a3f"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DEBUG_PrevCheckpoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b5cfad7-3f42-4b9a-88b4-d27f8fec0cfe"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DEBUG_PrevCheckpoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Comic"",
            ""id"": ""08a5f253-9d17-4bb6-97d9-dbb752b6dc71"",
            ""actions"": [
                {
                    ""name"": ""NextPage"",
                    ""type"": ""Value"",
                    ""id"": ""fab9747b-eb6a-42e3-8018-43e377c67edc"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""101c50e0-8652-4d17-a725-9fe987b0d90a"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""NextPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1711354f-8718-4942-a669-897206085cbe"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""NextPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f3a4350-3d16-4568-a89b-dcc78fe58dc7"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""NextPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63c05286-4ca8-4ce8-8464-b30bbf35f7ed"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Keypress"",
            ""id"": ""1132a19e-df41-41bd-8099-9fbbacf9b8a8"",
            ""actions"": [
                {
                    ""name"": ""L_Pressed"",
                    ""type"": ""Button"",
                    ""id"": ""714baed6-1ece-4cd1-afec-dda2a218421e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Y_Pressed"",
                    ""type"": ""Button"",
                    ""id"": ""82a58678-407d-4845-a50f-881133b3f4da"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""S_Pressed"",
                    ""type"": ""Button"",
                    ""id"": ""66b6ca37-e34e-4b5a-bed1-22987a0f2e70"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""U_Pressed"",
                    ""type"": ""Button"",
                    ""id"": ""8f23aa79-93eb-4d31-98ff-cd95c035b8aa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b1787add-def7-49de-8935-e98c37df23dd"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""L_Pressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""17385329-70a9-4ea6-966d-e5b8e0740ffe"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Y_Pressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43cea505-7630-4ee6-b8e9-fe80ac62e436"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""S_Pressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""932b6338-5e62-4b3b-bc3b-5607f944b435"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""U_Pressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Camera = m_Player.FindAction("Camera", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        m_Player_Recall = m_Player.FindAction("Recall", throwIfNotFound: true);
        m_Player_Throw = m_Player.FindAction("Throw", throwIfNotFound: true);
        m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
        m_Player_Crouch = m_Player.FindAction("Crouch", throwIfNotFound: true);
        m_Player_AimAssist = m_Player.FindAction("Aim Assist", throwIfNotFound: true);
        m_Player_SwitchHeld = m_Player.FindAction("Switch Held", throwIfNotFound: true);
        m_Player_Heal = m_Player.FindAction("Heal", throwIfNotFound: true);
        m_Player_ToggleDebugControls = m_Player.FindAction("Toggle Debug Controls", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Pause = m_Menu.FindAction("Pause", throwIfNotFound: true);
        m_Menu_Continue = m_Menu.FindAction("Continue", throwIfNotFound: true);
        m_Menu_Next = m_Menu.FindAction("Next", throwIfNotFound: true);
        m_Menu_ResetAll = m_Menu.FindAction("Reset All", throwIfNotFound: true);
        m_Menu_NextSubTab = m_Menu.FindAction("Next (Sub Tab)", throwIfNotFound: true);
        m_Menu_Previous = m_Menu.FindAction("Previous", throwIfNotFound: true);
        m_Menu_PreviousSubTab = m_Menu.FindAction("Previous (Sub Tab)", throwIfNotFound: true);
        m_Menu_Back = m_Menu.FindAction("Back", throwIfNotFound: true);
        // Cutscene
        m_Cutscene = asset.FindActionMap("Cutscene", throwIfNotFound: true);
        m_Cutscene_SkipCutscene = m_Cutscene.FindAction("SkipCutscene", throwIfNotFound: true);
        // Debug
        m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
        m_Debug_DEBUG_PrevLevel = m_Debug.FindAction("DEBUG_PrevLevel", throwIfNotFound: true);
        m_Debug_DEBUG_NextLevel = m_Debug.FindAction("DEBUG_NextLevel", throwIfNotFound: true);
        m_Debug_DEBUG_Pause = m_Debug.FindAction("DEBUG_Pause", throwIfNotFound: true);
        m_Debug_DEBUG_ReloadScene = m_Debug.FindAction("DEBUG_ReloadScene", throwIfNotFound: true);
        m_Debug_DEBUG_NextCheckpoint = m_Debug.FindAction("DEBUG_NextCheckpoint", throwIfNotFound: true);
        m_Debug_DEBUG_PrevCheckpoint = m_Debug.FindAction("DEBUG_PrevCheckpoint", throwIfNotFound: true);
        // Comic
        m_Comic = asset.FindActionMap("Comic", throwIfNotFound: true);
        m_Comic_NextPage = m_Comic.FindAction("NextPage", throwIfNotFound: true);
        // Keypress
        m_Keypress = asset.FindActionMap("Keypress", throwIfNotFound: true);
        m_Keypress_L_Pressed = m_Keypress.FindAction("L_Pressed", throwIfNotFound: true);
        m_Keypress_Y_Pressed = m_Keypress.FindAction("Y_Pressed", throwIfNotFound: true);
        m_Keypress_S_Pressed = m_Keypress.FindAction("S_Pressed", throwIfNotFound: true);
        m_Keypress_U_Pressed = m_Keypress.FindAction("U_Pressed", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Camera;
    private readonly InputAction m_Player_Dash;
    private readonly InputAction m_Player_Recall;
    private readonly InputAction m_Player_Throw;
    private readonly InputAction m_Player_Shoot;
    private readonly InputAction m_Player_Crouch;
    private readonly InputAction m_Player_AimAssist;
    private readonly InputAction m_Player_SwitchHeld;
    private readonly InputAction m_Player_Heal;
    private readonly InputAction m_Player_ToggleDebugControls;
    public struct PlayerActions
    {
        private @InputMaster m_Wrapper;
        public PlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Camera => m_Wrapper.m_Player_Camera;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputAction @Recall => m_Wrapper.m_Player_Recall;
        public InputAction @Throw => m_Wrapper.m_Player_Throw;
        public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
        public InputAction @Crouch => m_Wrapper.m_Player_Crouch;
        public InputAction @AimAssist => m_Wrapper.m_Player_AimAssist;
        public InputAction @SwitchHeld => m_Wrapper.m_Player_SwitchHeld;
        public InputAction @Heal => m_Wrapper.m_Player_Heal;
        public InputAction @ToggleDebugControls => m_Wrapper.m_Player_ToggleDebugControls;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Camera.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCamera;
                @Dash.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Recall.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRecall;
                @Recall.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRecall;
                @Recall.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRecall;
                @Throw.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrow;
                @Throw.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrow;
                @Throw.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThrow;
                @Shoot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Crouch.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @AimAssist.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAimAssist;
                @AimAssist.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAimAssist;
                @AimAssist.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAimAssist;
                @SwitchHeld.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchHeld;
                @SwitchHeld.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchHeld;
                @SwitchHeld.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchHeld;
                @Heal.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHeal;
                @Heal.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHeal;
                @Heal.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHeal;
                @ToggleDebugControls.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleDebugControls;
                @ToggleDebugControls.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleDebugControls;
                @ToggleDebugControls.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleDebugControls;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Recall.started += instance.OnRecall;
                @Recall.performed += instance.OnRecall;
                @Recall.canceled += instance.OnRecall;
                @Throw.started += instance.OnThrow;
                @Throw.performed += instance.OnThrow;
                @Throw.canceled += instance.OnThrow;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @AimAssist.started += instance.OnAimAssist;
                @AimAssist.performed += instance.OnAimAssist;
                @AimAssist.canceled += instance.OnAimAssist;
                @SwitchHeld.started += instance.OnSwitchHeld;
                @SwitchHeld.performed += instance.OnSwitchHeld;
                @SwitchHeld.canceled += instance.OnSwitchHeld;
                @Heal.started += instance.OnHeal;
                @Heal.performed += instance.OnHeal;
                @Heal.canceled += instance.OnHeal;
                @ToggleDebugControls.started += instance.OnToggleDebugControls;
                @ToggleDebugControls.performed += instance.OnToggleDebugControls;
                @ToggleDebugControls.canceled += instance.OnToggleDebugControls;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Pause;
    private readonly InputAction m_Menu_Continue;
    private readonly InputAction m_Menu_Next;
    private readonly InputAction m_Menu_ResetAll;
    private readonly InputAction m_Menu_NextSubTab;
    private readonly InputAction m_Menu_Previous;
    private readonly InputAction m_Menu_PreviousSubTab;
    private readonly InputAction m_Menu_Back;
    public struct MenuActions
    {
        private @InputMaster m_Wrapper;
        public MenuActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_Menu_Pause;
        public InputAction @Continue => m_Wrapper.m_Menu_Continue;
        public InputAction @Next => m_Wrapper.m_Menu_Next;
        public InputAction @ResetAll => m_Wrapper.m_Menu_ResetAll;
        public InputAction @NextSubTab => m_Wrapper.m_Menu_NextSubTab;
        public InputAction @Previous => m_Wrapper.m_Menu_Previous;
        public InputAction @PreviousSubTab => m_Wrapper.m_Menu_PreviousSubTab;
        public InputAction @Back => m_Wrapper.m_Menu_Back;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnPause;
                @Continue.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnContinue;
                @Continue.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnContinue;
                @Continue.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnContinue;
                @Next.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnNext;
                @Next.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnNext;
                @Next.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnNext;
                @ResetAll.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnResetAll;
                @ResetAll.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnResetAll;
                @ResetAll.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnResetAll;
                @NextSubTab.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnNextSubTab;
                @NextSubTab.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnNextSubTab;
                @NextSubTab.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnNextSubTab;
                @Previous.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnPrevious;
                @Previous.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnPrevious;
                @Previous.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnPrevious;
                @PreviousSubTab.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnPreviousSubTab;
                @PreviousSubTab.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnPreviousSubTab;
                @PreviousSubTab.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnPreviousSubTab;
                @Back.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Continue.started += instance.OnContinue;
                @Continue.performed += instance.OnContinue;
                @Continue.canceled += instance.OnContinue;
                @Next.started += instance.OnNext;
                @Next.performed += instance.OnNext;
                @Next.canceled += instance.OnNext;
                @ResetAll.started += instance.OnResetAll;
                @ResetAll.performed += instance.OnResetAll;
                @ResetAll.canceled += instance.OnResetAll;
                @NextSubTab.started += instance.OnNextSubTab;
                @NextSubTab.performed += instance.OnNextSubTab;
                @NextSubTab.canceled += instance.OnNextSubTab;
                @Previous.started += instance.OnPrevious;
                @Previous.performed += instance.OnPrevious;
                @Previous.canceled += instance.OnPrevious;
                @PreviousSubTab.started += instance.OnPreviousSubTab;
                @PreviousSubTab.performed += instance.OnPreviousSubTab;
                @PreviousSubTab.canceled += instance.OnPreviousSubTab;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);

    // Cutscene
    private readonly InputActionMap m_Cutscene;
    private ICutsceneActions m_CutsceneActionsCallbackInterface;
    private readonly InputAction m_Cutscene_SkipCutscene;
    public struct CutsceneActions
    {
        private @InputMaster m_Wrapper;
        public CutsceneActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @SkipCutscene => m_Wrapper.m_Cutscene_SkipCutscene;
        public InputActionMap Get() { return m_Wrapper.m_Cutscene; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CutsceneActions set) { return set.Get(); }
        public void SetCallbacks(ICutsceneActions instance)
        {
            if (m_Wrapper.m_CutsceneActionsCallbackInterface != null)
            {
                @SkipCutscene.started -= m_Wrapper.m_CutsceneActionsCallbackInterface.OnSkipCutscene;
                @SkipCutscene.performed -= m_Wrapper.m_CutsceneActionsCallbackInterface.OnSkipCutscene;
                @SkipCutscene.canceled -= m_Wrapper.m_CutsceneActionsCallbackInterface.OnSkipCutscene;
            }
            m_Wrapper.m_CutsceneActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SkipCutscene.started += instance.OnSkipCutscene;
                @SkipCutscene.performed += instance.OnSkipCutscene;
                @SkipCutscene.canceled += instance.OnSkipCutscene;
            }
        }
    }
    public CutsceneActions @Cutscene => new CutsceneActions(this);

    // Debug
    private readonly InputActionMap m_Debug;
    private IDebugActions m_DebugActionsCallbackInterface;
    private readonly InputAction m_Debug_DEBUG_PrevLevel;
    private readonly InputAction m_Debug_DEBUG_NextLevel;
    private readonly InputAction m_Debug_DEBUG_Pause;
    private readonly InputAction m_Debug_DEBUG_ReloadScene;
    private readonly InputAction m_Debug_DEBUG_NextCheckpoint;
    private readonly InputAction m_Debug_DEBUG_PrevCheckpoint;
    public struct DebugActions
    {
        private @InputMaster m_Wrapper;
        public DebugActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @DEBUG_PrevLevel => m_Wrapper.m_Debug_DEBUG_PrevLevel;
        public InputAction @DEBUG_NextLevel => m_Wrapper.m_Debug_DEBUG_NextLevel;
        public InputAction @DEBUG_Pause => m_Wrapper.m_Debug_DEBUG_Pause;
        public InputAction @DEBUG_ReloadScene => m_Wrapper.m_Debug_DEBUG_ReloadScene;
        public InputAction @DEBUG_NextCheckpoint => m_Wrapper.m_Debug_DEBUG_NextCheckpoint;
        public InputAction @DEBUG_PrevCheckpoint => m_Wrapper.m_Debug_DEBUG_PrevCheckpoint;
        public InputActionMap Get() { return m_Wrapper.m_Debug; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugActions set) { return set.Get(); }
        public void SetCallbacks(IDebugActions instance)
        {
            if (m_Wrapper.m_DebugActionsCallbackInterface != null)
            {
                @DEBUG_PrevLevel.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_PrevLevel;
                @DEBUG_PrevLevel.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_PrevLevel;
                @DEBUG_PrevLevel.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_PrevLevel;
                @DEBUG_NextLevel.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_NextLevel;
                @DEBUG_NextLevel.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_NextLevel;
                @DEBUG_NextLevel.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_NextLevel;
                @DEBUG_Pause.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_Pause;
                @DEBUG_Pause.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_Pause;
                @DEBUG_Pause.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_Pause;
                @DEBUG_ReloadScene.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_ReloadScene;
                @DEBUG_ReloadScene.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_ReloadScene;
                @DEBUG_ReloadScene.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_ReloadScene;
                @DEBUG_NextCheckpoint.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_NextCheckpoint;
                @DEBUG_NextCheckpoint.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_NextCheckpoint;
                @DEBUG_NextCheckpoint.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_NextCheckpoint;
                @DEBUG_PrevCheckpoint.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_PrevCheckpoint;
                @DEBUG_PrevCheckpoint.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_PrevCheckpoint;
                @DEBUG_PrevCheckpoint.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnDEBUG_PrevCheckpoint;
            }
            m_Wrapper.m_DebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @DEBUG_PrevLevel.started += instance.OnDEBUG_PrevLevel;
                @DEBUG_PrevLevel.performed += instance.OnDEBUG_PrevLevel;
                @DEBUG_PrevLevel.canceled += instance.OnDEBUG_PrevLevel;
                @DEBUG_NextLevel.started += instance.OnDEBUG_NextLevel;
                @DEBUG_NextLevel.performed += instance.OnDEBUG_NextLevel;
                @DEBUG_NextLevel.canceled += instance.OnDEBUG_NextLevel;
                @DEBUG_Pause.started += instance.OnDEBUG_Pause;
                @DEBUG_Pause.performed += instance.OnDEBUG_Pause;
                @DEBUG_Pause.canceled += instance.OnDEBUG_Pause;
                @DEBUG_ReloadScene.started += instance.OnDEBUG_ReloadScene;
                @DEBUG_ReloadScene.performed += instance.OnDEBUG_ReloadScene;
                @DEBUG_ReloadScene.canceled += instance.OnDEBUG_ReloadScene;
                @DEBUG_NextCheckpoint.started += instance.OnDEBUG_NextCheckpoint;
                @DEBUG_NextCheckpoint.performed += instance.OnDEBUG_NextCheckpoint;
                @DEBUG_NextCheckpoint.canceled += instance.OnDEBUG_NextCheckpoint;
                @DEBUG_PrevCheckpoint.started += instance.OnDEBUG_PrevCheckpoint;
                @DEBUG_PrevCheckpoint.performed += instance.OnDEBUG_PrevCheckpoint;
                @DEBUG_PrevCheckpoint.canceled += instance.OnDEBUG_PrevCheckpoint;
            }
        }
    }
    public DebugActions @Debug => new DebugActions(this);

    // Comic
    private readonly InputActionMap m_Comic;
    private IComicActions m_ComicActionsCallbackInterface;
    private readonly InputAction m_Comic_NextPage;
    public struct ComicActions
    {
        private @InputMaster m_Wrapper;
        public ComicActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @NextPage => m_Wrapper.m_Comic_NextPage;
        public InputActionMap Get() { return m_Wrapper.m_Comic; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ComicActions set) { return set.Get(); }
        public void SetCallbacks(IComicActions instance)
        {
            if (m_Wrapper.m_ComicActionsCallbackInterface != null)
            {
                @NextPage.started -= m_Wrapper.m_ComicActionsCallbackInterface.OnNextPage;
                @NextPage.performed -= m_Wrapper.m_ComicActionsCallbackInterface.OnNextPage;
                @NextPage.canceled -= m_Wrapper.m_ComicActionsCallbackInterface.OnNextPage;
            }
            m_Wrapper.m_ComicActionsCallbackInterface = instance;
            if (instance != null)
            {
                @NextPage.started += instance.OnNextPage;
                @NextPage.performed += instance.OnNextPage;
                @NextPage.canceled += instance.OnNextPage;
            }
        }
    }
    public ComicActions @Comic => new ComicActions(this);

    // Keypress
    private readonly InputActionMap m_Keypress;
    private IKeypressActions m_KeypressActionsCallbackInterface;
    private readonly InputAction m_Keypress_L_Pressed;
    private readonly InputAction m_Keypress_Y_Pressed;
    private readonly InputAction m_Keypress_S_Pressed;
    private readonly InputAction m_Keypress_U_Pressed;
    public struct KeypressActions
    {
        private @InputMaster m_Wrapper;
        public KeypressActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @L_Pressed => m_Wrapper.m_Keypress_L_Pressed;
        public InputAction @Y_Pressed => m_Wrapper.m_Keypress_Y_Pressed;
        public InputAction @S_Pressed => m_Wrapper.m_Keypress_S_Pressed;
        public InputAction @U_Pressed => m_Wrapper.m_Keypress_U_Pressed;
        public InputActionMap Get() { return m_Wrapper.m_Keypress; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeypressActions set) { return set.Get(); }
        public void SetCallbacks(IKeypressActions instance)
        {
            if (m_Wrapper.m_KeypressActionsCallbackInterface != null)
            {
                @L_Pressed.started -= m_Wrapper.m_KeypressActionsCallbackInterface.OnL_Pressed;
                @L_Pressed.performed -= m_Wrapper.m_KeypressActionsCallbackInterface.OnL_Pressed;
                @L_Pressed.canceled -= m_Wrapper.m_KeypressActionsCallbackInterface.OnL_Pressed;
                @Y_Pressed.started -= m_Wrapper.m_KeypressActionsCallbackInterface.OnY_Pressed;
                @Y_Pressed.performed -= m_Wrapper.m_KeypressActionsCallbackInterface.OnY_Pressed;
                @Y_Pressed.canceled -= m_Wrapper.m_KeypressActionsCallbackInterface.OnY_Pressed;
                @S_Pressed.started -= m_Wrapper.m_KeypressActionsCallbackInterface.OnS_Pressed;
                @S_Pressed.performed -= m_Wrapper.m_KeypressActionsCallbackInterface.OnS_Pressed;
                @S_Pressed.canceled -= m_Wrapper.m_KeypressActionsCallbackInterface.OnS_Pressed;
                @U_Pressed.started -= m_Wrapper.m_KeypressActionsCallbackInterface.OnU_Pressed;
                @U_Pressed.performed -= m_Wrapper.m_KeypressActionsCallbackInterface.OnU_Pressed;
                @U_Pressed.canceled -= m_Wrapper.m_KeypressActionsCallbackInterface.OnU_Pressed;
            }
            m_Wrapper.m_KeypressActionsCallbackInterface = instance;
            if (instance != null)
            {
                @L_Pressed.started += instance.OnL_Pressed;
                @L_Pressed.performed += instance.OnL_Pressed;
                @L_Pressed.canceled += instance.OnL_Pressed;
                @Y_Pressed.started += instance.OnY_Pressed;
                @Y_Pressed.performed += instance.OnY_Pressed;
                @Y_Pressed.canceled += instance.OnY_Pressed;
                @S_Pressed.started += instance.OnS_Pressed;
                @S_Pressed.performed += instance.OnS_Pressed;
                @S_Pressed.canceled += instance.OnS_Pressed;
                @U_Pressed.started += instance.OnU_Pressed;
                @U_Pressed.performed += instance.OnU_Pressed;
                @U_Pressed.canceled += instance.OnU_Pressed;
            }
        }
    }
    public KeypressActions @Keypress => new KeypressActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnRecall(InputAction.CallbackContext context);
        void OnThrow(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnAimAssist(InputAction.CallbackContext context);
        void OnSwitchHeld(InputAction.CallbackContext context);
        void OnHeal(InputAction.CallbackContext context);
        void OnToggleDebugControls(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnPause(InputAction.CallbackContext context);
        void OnContinue(InputAction.CallbackContext context);
        void OnNext(InputAction.CallbackContext context);
        void OnResetAll(InputAction.CallbackContext context);
        void OnNextSubTab(InputAction.CallbackContext context);
        void OnPrevious(InputAction.CallbackContext context);
        void OnPreviousSubTab(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
    }
    public interface ICutsceneActions
    {
        void OnSkipCutscene(InputAction.CallbackContext context);
    }
    public interface IDebugActions
    {
        void OnDEBUG_PrevLevel(InputAction.CallbackContext context);
        void OnDEBUG_NextLevel(InputAction.CallbackContext context);
        void OnDEBUG_Pause(InputAction.CallbackContext context);
        void OnDEBUG_ReloadScene(InputAction.CallbackContext context);
        void OnDEBUG_NextCheckpoint(InputAction.CallbackContext context);
        void OnDEBUG_PrevCheckpoint(InputAction.CallbackContext context);
    }
    public interface IComicActions
    {
        void OnNextPage(InputAction.CallbackContext context);
    }
    public interface IKeypressActions
    {
        void OnL_Pressed(InputAction.CallbackContext context);
        void OnY_Pressed(InputAction.CallbackContext context);
        void OnS_Pressed(InputAction.CallbackContext context);
        void OnU_Pressed(InputAction.CallbackContext context);
    }
}

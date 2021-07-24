module Main exposing (main)

import Browser
import Browser.Navigation as Nav
import Html               exposing (Html, article, button, div, h1, h2, header, input, main_, section, text)
import Html.Attributes    exposing (value)
import Html.Events        exposing (onClick, onInput)
import Html.Lazy          exposing (lazy)
import Http
import Json.Decode
import Json.Encode
import String.Format      as Format
import Url
import Url.Parser         exposing (Parser, (</>), map, oneOf, parse, s, string, top)

main : Program () Model Msg
main = Browser.application
    { init          = init
    , view          = view
    , update        = update
    , subscriptions = subscriptions
    , onUrlRequest  = UrlRequest
    , onUrlChange   = UrlChange
    }

type alias Model =
    { key       : Nav.Key
    , route     : Route
    , loading   : Bool
    , name      : String
    , helloText : String
    }

type Route
    = NotFoundPage
    | IndexPage
    | HelloPage String

type Msg
    = None
    | UrlRequest   Browser.UrlRequest
    | UrlChange    Url.Url
    | Navigate     String
    | SetLoading   Bool
    | SetName      String
    | SendHello    String
    | ReceiveHello String

init : () -> Url.Url -> Nav.Key -> (Model, Cmd Msg)
init _ url key =
    (Model key (toRoute url) False "world" (helloString "world"), Cmd.none)

update : Msg -> Model -> (Model, Cmd Msg)
update msg model =
    case msg of
        None ->
            (model, Cmd.none)
        UrlRequest req ->
            case req of
                Browser.External href ->
                    (model, Nav.load href)
                Browser.Internal url ->
                    (model, Nav.pushUrl model.key (Url.toString url))
        UrlChange url ->
            let route = toRoute url in
                case route of
                    HelloPage name ->
                        ({ model | route = route, name = name }, Cmd.none)
                    _ ->
                        ({ model | route = route }, Cmd.none)
        Navigate url ->
            (model, Nav.pushUrl model.key url)
        SetLoading loading ->
            ({ model | loading = loading }, Cmd.none)
        SetName name ->
            ({ model | name = name }, Cmd.none)
        SendHello name ->
            (model, sendHello name)
        ReceiveHello hello ->
            ({ model | helloText = hello }, Cmd.none)

toRoute : Url.Url -> Route
toRoute url =
    case parse routeParser url of
        Just page -> page
        Nothing   -> NotFoundPage

routeParser : Parser (Route -> a) a
routeParser =
    oneOf
        [ map IndexPage top
        , map (HelloPage "world") <| s "hello"
        , map HelloPage <| s "hello" </> string
        ]

sendHello : String -> Cmd Msg
sendHello name =
    Http.post
        { url = "/api/home/hello"
        , body = Http.jsonBody <| nameEncoder name
        , expect = Http.expectJson receiveHello helloDecoder
        }

nameEncoder : String -> Json.Encode.Value
nameEncoder name = Json.Encode.object [("name", Json.Encode.string name)]

helloDecoder : Json.Decode.Decoder String
helloDecoder = Json.Decode.field "hello" Json.Decode.string

receiveHello : Result Http.Error String -> Msg
receiveHello result =
    case result of
        Ok name -> ReceiveHello name
        Err _ -> None

subscriptions : Model -> Sub Msg
subscriptions _ = Sub.none

view : Model -> Browser.Document Msg
view model =
    { title =
        case model.route of
            NotFoundPage -> "Elm Page - Not Found"
            IndexPage    -> "Elm Page"
            HelloPage _  -> "Elm Page - {{ hello }}" |> Format.namedValue "hello" model.helloText
    , body =
        [ header []
            [ div []
                [ h1 [] [ text "Elm Page" ]
                ]
            ]
        , main_ []
            [
                if model.loading
                then lazy text "Loading..."
                else lazy viewRoute model
            ]
        ]
    }

viewRoute : Model -> Html Msg
viewRoute model =
    case model.route of
        NotFoundPage -> viewNotFoundPage model
        IndexPage    -> viewIndexPage    model
        HelloPage _  -> viewHelloPage    model

viewNotFoundPage : Model -> Html Msg
viewNotFoundPage _ =
    article []
        [ div []
            [ h1 [] [ text "Not Found" ]
            ]
        ]

viewIndexPage : Model -> Html Msg
viewIndexPage _ =
    article []
        [ div []
            [ button [ onClick <| Navigate "/hello" ] [ text "Hello" ]
            ]
        ]

viewHelloPage : Model -> Html Msg
viewHelloPage model =
    div []
        [ article []
            [ section []
                [ div []
                    [ h2 [] [ text <| model.helloText ]
                    , input [ value model.name, onInput SetName ] []
                    , button [ onClick <| SendHello <| model.name ] [ text "Hello" ]
                    ]
                ]
            ]
        ]

helloString : String -> String
helloString name =
    "Hello, {{ name }}!"
    |> Format.namedValue "name"  name

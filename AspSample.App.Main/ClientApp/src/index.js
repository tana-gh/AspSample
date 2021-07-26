import { Elm } from './elm/Main.elm'
import firebase from 'firebase'
import * as firebaseui from 'firebaseui'
import 'firebaseui/dist/firebaseui.css'
import '../assets/scss/style.scss'

fetch(new Request('/api/key/firebase'), {
    method: 'GET',
    headers: new Headers([
        [ 'Accept', 'application/json' ]
    ])
})
.then(res => res.json())
.then(json => {
    firebase.initializeApp(json)
    firebase.analytics()

    class AttachFirebaseuiAuth extends HTMLElement {
        constructor() {
            super()
            
            setTimeout(() => {
                const target = document.getElementById('firebaseui-auth-container')

                if (!target) return

                const ui = new firebaseui.auth.AuthUI(firebase.auth())

                ui.start(target, {
                    signInFlow: 'popup',
                    signInOptions: [
                        {
                            provider: firebase.auth.EmailAuthProvider.PROVIDER_ID,
                            signInMethod: firebase.auth.EmailAuthProvider.EMAIL_PASSWORD_SIGN_IN_METHOD
                        },
                        {
                            provider: firebase.auth.GoogleAuthProvider.PROVIDER_ID,
                            customParameters: {
                                prompt: 'select_account'
                            }
                        }
                    ],
                    tosUrl: '',
                    privacyPolicyUrl: '',
                    callbacks: {
                        signInSuccessWithAuthResult: () => {
                            firebase.auth().currentUser.getIdToken(true)
                                .then(idToken => {
                                    app.ports.authPort.send(idToken)
                                })
                                .catch(() => {
                                    app.ports.errorPort.send('ID token not available')
                                })
                            return false;
                        }
                    }
                })
            }, 0)
        }
    }

    customElements.define('attach-firebaseui-auth', AttachFirebaseuiAuth)

    const app = Elm.Main.init({
        node: document.getElementById('app')
    })
})

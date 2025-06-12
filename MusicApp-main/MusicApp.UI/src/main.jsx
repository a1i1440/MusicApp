import { createRoot } from 'react-dom/client'
import AppWrapper from './App.jsx'
import { Provider } from 'react-redux'
import store from './store/store.js'
import '../node_modules/bootstrap/dist/css/bootstrap.css'

createRoot(document.getElementById('root')).render(
  <Provider store={store}>
    <AppWrapper />
  </Provider>,
)

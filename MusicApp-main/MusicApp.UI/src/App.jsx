import './App.css'
import api, { setupAxiosInterceptors } from './api'
import { useEffect, useState } from 'react'
import { baseURL } from './assets/baseEnvironment'
import { Route, BrowserRouter as Router, Routes, useNavigate } from 'react-router-dom';
import LoginRegister from './components/LoginRegister';
import DefaultLayout from './components/DefaultLayout';
import MainPage from './components/MainPage';

function App() {
  const navigate = useNavigate();
  useEffect(() => {
    setupAxiosInterceptors(navigate)
  }, [])

  return (
    <Routes>
      <Route path='/' element={localStorage.getItem("TOKEN_KEY") ? <DefaultLayout /> : <LoginRegister />}>
        <Route index element={<MainPage/>}/>
      </Route>
    </Routes>
  )
}

function AppWrapper(){
  return (
    <Router>
      <App />
    </Router>
  )
}

export default AppWrapper

import { useState } from 'react'
import logo from './logo.svg'
import './App.css'
import { Provider, useSelector } from 'react-redux'
import { store } from './bootstrap/store'
import { getUserInfos } from './store/users/usersSelectors'

function App() {
  const [count, setCount] = useState(0);

  return (
    <Provider store={store}>
      <div className="App">
        <UserInfos />
      </div>
    </Provider>
    
  )
}

function UserInfos() {
  const userInfos = useSelector(getUserInfos);
  return (
    <pre>{JSON.stringify(userInfos)}</pre>
  )
}

export default App

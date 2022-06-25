import { Provider, useSelector } from 'react-redux';
import { store } from './bootstrap/store';
import { getUserInfos } from './store/users/usersSelectors';
import { Header } from './components/layout/Header';
import { SkuldRouter } from './router';

function App() {
  return (
    <Provider store={store}>
      <div className="App">
        <Header />
        <SkuldRouter/>
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

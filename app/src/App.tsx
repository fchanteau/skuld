import { Provider, useSelector } from 'react-redux';
import { store } from './bootstrap/store';
import { SkuldRouter } from './router';
import { Sidebar } from './components/layout/Sidebar';
import { Col, Container, Row } from 'reactstrap';
import { UserInfos } from './components/layout/UserInfos';

function App() {
  return (
    <Provider store={store}>
      <div className="App d-flex align-items-stretch">
        <LeftBar />
        <MainLayout />
      </div>
    </Provider>
    
  )
}

function LeftBar() {
  return (
    <div className='min-vh-100 left-bar'>
      <UserInfos />
      <Sidebar />
    </div>
  )
}

function MainLayout() {
  return (
    <div className='flex-grow-1'>
      <SkuldRouter />
    </div>
  )
}

export default App

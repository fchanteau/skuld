import { useSelector } from 'react-redux';
import { Sidebar } from './components/layout/Sidebar';
import { TopBar } from './components/layout/TopBar';
import { MainLayout } from './components/layout/MainLayout';
import { isConnected } from './store/auth/authSelectors';
import { LandingPage } from './components/layout/LandingPage';

function App() {
  const userIsConnected = useSelector(isConnected);

  if (!userIsConnected) {
    return <LandingPage />
  }

  return (
    <>
    <TopBar />
      <div className='d-flex align-items-stretch'>
        <Sidebar />
        <MainLayout />
      </div>
    </>
  )
}

export default App;
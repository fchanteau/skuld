import { useSelector } from 'react-redux';
import { Sidebar, TopBar, MainLayout, LandingPage } from '@/components/layout';
import { isConnected } from '@/store/auth';

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
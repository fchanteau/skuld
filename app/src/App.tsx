import { Provider, useSelector } from 'react-redux';
import { store } from './bootstrap/store';
import { Sidebar } from './components/layout/Sidebar';
import { TopBar } from './components/layout/TopBar';
import { MainLayout } from './components/layout/MainLayout';

function App() {
  return (
    <Provider store={store}>
      <div className="App min-vh-100">
        <TopBar />
        <div className='d-flex align-items-stretch'>
          <Sidebar />
          <MainLayout />
        </div>
      </div>
    </Provider>
    
  )
}

export default App

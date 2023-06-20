import { Provider, useSelector } from 'react-redux';
import { store } from './bootstrap';
import { RouterProvider } from 'react-router-dom';
import { router } from './pages/router';
import { StrictMode } from 'react';

const MainContainer = (): JSX.Element => {
  return <RouterProvider router={router} />
}

const App = (): JSX.Element => {
  return (
    <Provider store={store}>
      <StrictMode>
        <MainContainer />
      </StrictMode>
    </Provider>
  )
}

export default App;
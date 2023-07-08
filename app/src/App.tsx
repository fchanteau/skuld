import { StrictMode } from 'react';
import { Provider } from 'react-redux';
import { RouterProvider } from 'react-router-dom';

import { router } from './pages/router';
import { store } from './bootstrap';

const MainContainer = (): JSX.Element => {
  return <RouterProvider router={router} />;
};

const App = (): JSX.Element => {
  return (
    <Provider store={store}>
      <StrictMode>
        <MainContainer />
      </StrictMode>
    </Provider>
  );
};

export default App;

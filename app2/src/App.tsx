import { ChakraProvider } from "@chakra-ui/react";
import { StrictMode } from "react";
import { Provider } from "react-redux";

import { store } from "./common/store/store";
import { AuthPage } from "./pages/AuthPage";

function App(): JSX.Element {
  return (
    <StrictMode>
      <Provider store={store}>
        <ChakraProvider>
          <AuthPage />
        </ChakraProvider>
      </Provider>
    </StrictMode>
  );
}

export default App;

import { createSkuldApi } from "../api";
import { createStore } from "../store/store";

export const store = createStore;

export const initStore = () => {
    store.dispatch(async (dispatch, _getState, { actionCreators }) => {
        await dispatch(actionCreators.users.login({email: "francois.chanteau49@gmail.com", password: "Carapuce49"}));
    });
}

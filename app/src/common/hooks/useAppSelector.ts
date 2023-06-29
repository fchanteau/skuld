import { type TypedUseSelectorHook, useSelector } from "react-redux";

import { type AppState } from "@/bootstrap";

export const useAppSelector: TypedUseSelectorHook<AppState> = useSelector;
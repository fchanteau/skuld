import { useDispatch } from "react-redux";

import { type AppDispatch } from "@/bootstrap";

export const useAppDispatch = () => useDispatch<AppDispatch>();
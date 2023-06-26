import { AppDispatch } from "@/bootstrap";
import { useDispatch } from "react-redux";

export const useAppDispatch = () => useDispatch<AppDispatch>();
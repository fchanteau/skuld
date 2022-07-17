import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Home } from "./components/features/dashboard/Home";

export function SkuldRouter() {
    return (
        <Routes>
            <Route path="/" element={<Home />}/>
            <Route path="/about" element={<h1>About page</h1>}/>
        </Routes>
    )
}
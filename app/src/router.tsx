import { BrowserRouter, Route, Routes } from "react-router-dom";

export function SkuldRouter() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<h1>Home page</h1>}/>
                <Route path="/about" element={<h1>About page</h1>}/>
            </Routes>
        </BrowserRouter>
    )
}
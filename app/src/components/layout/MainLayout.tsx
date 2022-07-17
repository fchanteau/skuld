import { SkuldRouter } from "@/router";

export function MainLayout() {
    return (
      <div className='flex-grow-1 main'>
        <SkuldRouter />
      </div>
    )
  }
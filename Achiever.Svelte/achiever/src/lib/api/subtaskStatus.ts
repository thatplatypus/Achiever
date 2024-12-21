import { Circle, CircleArrowUp, CircleCheck } from "lucide-svelte";
import type { ComponentType } from "svelte";

type SubTaskStatusValue = "New" | "InProgress" | "Completed";


interface SubTaskStatus {
    value: SubTaskStatusValue;
    label: string;
    icon: ComponentType;
  }
  
  export const statuses: SubTaskStatus[] = [
    { value: "New", label: "New", icon: Circle },
    { value: "InProgress", label: "In Progress", icon: CircleArrowUp },
    { value: "Completed", label: "Completed", icon: CircleCheck },
  ];
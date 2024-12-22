export type Goal = {
    id: string;
    title: string;
    subTasks: SubTask[];
    status: number;
    targetEndDate?: Date | string;
  };
  
export type SubTask = {
    id: string;
    title: string;
    status: string;
    userDeleted?: boolean;
    estimatedHours?: number;
    lastModified?: Date;
    order?: number;
    note?: string;
  };
  
  type APIResult<T> = { isSuccess: boolean; value?: T; message?: string };
  
  // Fetch all goals
  export async function getGoals(): Promise<APIResult<Goal[]>> {
    try {
      const response = await fetch('https://localhost:7211/GetGoals', {
        method: 'GET',
        credentials: 'include', // Include cookies
      });
  
      if (!response.ok) {
        throw new Error(`Failed to fetch goals: ${response.status}`);
      }
  
      const data = await response.json();
      return { isSuccess: true, value: data.goals };
    } catch (error) {
      return { isSuccess: false, message: (error as Error).message };
    }
  }
  
  // Create a new goal
  export async function createGoal(title: string): Promise<APIResult<Goal>> {
    try {
      const response = await fetch('https://localhost:7211/CreateGoal', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({ goal: { title } }),
      });
  
      if (!response.ok) {
        throw new Error(`Failed to create goal: ${response.status}`);
      }
  
      const data = await response.json();
      return {
        isSuccess: true,
        value: { id: data.id, title, subTasks: [], status: 0 },
      };
    } catch (error) {
      return { isSuccess: false, message: (error as Error).message };
    }
  }
  
  // Update an existing goal
  export async function updateGoal(goal: Goal): Promise<APIResult<string>> {
    try {
      const response = await fetch('https://localhost:7211/UpdateGoal', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({ goal }),
      });
  
      if (!response.ok) {
        throw new Error(`Failed to update goal: ${response.status}`);
      }
  
      const data = await response.json();
      return { isSuccess: true, value: data.id };
    } catch (error) {
      return { isSuccess: false, message: (error as Error).message };
    }
  }
  
  // Delete a goal
  export async function deleteGoal(goalId: string): Promise<APIResult<boolean>> {
    try {
      const response = await fetch('https://localhost:7211/DeleteGoal', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({ goalId }),
      });
  
      if (!response.ok) {
        throw new Error(`Failed to delete goal: ${response.status}`);
      }
  
      const data = await response.json();
      return { isSuccess: true, value: data.success };
    } catch (error) {
      return { isSuccess: false, message: (error as Error).message };
    }
  }
  
  // Create a subtask for a goal
  export async function createSubTask(goalId: string, subTask: SubTask): Promise<APIResult<string>> {
    try {
      const response = await fetch('https://localhost:7211/CreateSubTask', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({ goalId, subTask }),
      });
  
      if (!response.ok) {
        throw new Error(`Failed to create subtask: ${response.status}`);
      }
  
      const data = await response.json();
      return { isSuccess: true, value: data.subtaskId };
    } catch (error) {
      return { isSuccess: false, message: (error as Error).message };
    }
  }
  
  // Delete a subtask
  export async function deleteSubTask(subTaskId: string): Promise<APIResult<boolean>> {
    try {
      const response = await fetch('https://localhost:7211/DeleteSubTask', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({ subTaskId }),
      });
  
      if (!response.ok) {
        throw new Error(`Failed to delete subtask: ${response.status}`);
      }
  
      const data = await response.json();
      return { isSuccess: true, value: data.success };
    } catch (error) {
      return { isSuccess: false, message: (error as Error).message };
    }
  }
  
  export async function getGoalById(id: string): Promise<APIResult<Goal>> {
    try {
      const response = await fetch(
        `https://localhost:7211/GetGoalById?request=${encodeURIComponent(
          JSON.stringify({ id })
        )}`,
        {
          method: 'GET',
          credentials: 'include',
        }
      );
  
      if (!response.ok) {
        throw new Error(`Failed to fetch goal: ${response.status}`);
      }
  
      const data = await response.json();
      return { isSuccess: true, value: data.goal };
    } catch (error) {
      return { isSuccess: false, message: (error as Error).message };
    }
  }
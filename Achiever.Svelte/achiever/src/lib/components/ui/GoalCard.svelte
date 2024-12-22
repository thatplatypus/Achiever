<script lang="ts">
  import { Button } from "$lib/components/ui/button/index.js";
  import * as Card from "$lib/components/ui/card/index.js";
  import * as Tooltip from "$lib/components/ui/tooltip/index.js";
  import { Label } from "$lib/components/ui/label/index.js";
  import { goto } from "$app/navigation";
  import TooltipIconButton from "./TooltipIconButton.svelte";
  import EditGoalDialog from "./EditGoalDialog.svelte";
  import Pencil from "lucide-svelte/icons/pencil";
  import Trash from "lucide-svelte/icons/trash";
  import { updateGoal, type Goal, type SubTask } from "$lib/api/goals";
    import { onMount } from "svelte";
    import { Calendar, Clock } from "lucide-svelte";
    import { ScrollArea } from "./scroll-area";
    import SubtaskMiniCard from "./SubtaskMiniCard.svelte";
    import { Trigger } from "./dialog";

  export let goal: Goal;

  export let onDelete: (goalId: string) => void;

  let chart; // ApexCharts instance
  let chartOptions = {}; // Chart options
  let percentageCompleted = 0;

  // Calculate percentage of completed subtasks
  function calculatePercentage() {
    const totalSubtasks = goal.subTasks.length;
    if (totalSubtasks === 0) {
      return 0; // No subtasks
    }
    const completedSubtasks = goal.subTasks.filter((task) => task.status === "Completed").length;
    return Math.round((completedSubtasks / totalSubtasks) * 100);
  }

  // Handle deleting the goal
  function handleDelete() {
    onDelete(goal.id);
  }

  // Handle navigating to the goal details page
  function handleCardClick(event: MouseEvent) {
    const isButtonClick = (event.target as HTMLElement).closest("button");
    if (!isButtonClick) {
      handleNavigate(goal.id);
    }
  }

  function handleNavigate(goalId: string) {
    goto(`/goals/${goalId}`);
  }

  // Handle updating the goal
  async function handleGoalUpdated(id: string, title: string, status: number, targetEndDate?: Date) {
    console.log("Goal updated:", { id, title, status });
    goal.title = title;
    goal.status = status;
    if(targetEndDate)
      goal.targetEndDate = targetEndDate;
  
      const result = await updateGoal(goal);
    if (result.isSuccess) {
      console.log("Goal successfully updated!");
    } else {
      alert(`Failed to update goal: ${result.message}`);
    }
  }

  function getColorByPercentage(percentage: number) : string {
  switch (true) {
    case percentage === 100:
      return "#16A34A"; // Emerald for 100%
    case percentage >= 90:
      return "#082f49"; // Sky-950
    case percentage >= 80:
      return "#0c4a6e"; // Sky-900
    case percentage >= 70:
      return "#075985"; // Sky-800
    case percentage >= 60:
      return "#0369a1"; // Sky-700
    case percentage >= 50:
      return "#0284c7"; // Sky-600
    case percentage >= 40:
      return "#0ea5e9"; // Sky-500
    case percentage >= 30:
      return "#38bdf8"; // Sky-400
    case percentage >= 20:
      return "#7dd3fc"; // Sky-300
    default:
      return "#7dd3fc";
  }
}

  // Setup chart options
  $: percentageCompleted = calculatePercentage();
  $: chartOptions = {
  series: [percentageCompleted, 100 - percentageCompleted],
  chart: {
    type: "donut",
    height: 150,
  },
  labels: ["Completed", "Remaining"],
  colors: [getColorByPercentage(percentageCompleted), "#D1D5DB"],
  plotOptions: {
    pie: {
      donut: {
        size: "70%",
        labels: {
          show: true,
          name: {
            show: false,
          },
          total: {
            show: true,
            label: `${percentageCompleted}%`,
            fontSize: "16px",
            fontFamily: "Inter, sans-serif",
            fontWeight: 600,
            color: "#111827", // Dark text color
            formatter: () => `${percentageCompleted}%`,
          },
          value: {
            show: true,
            formatter: ( ) => {
              return `${percentageCompleted}%`;
          },
        },
        },
      },
    },
  },
  dataLabels: {
    enabled: false,
  },
  legend: {
    show: false,
  },
  tooltip: {
    enabled: true,
    custom: function ({ seriesIndex, series, w }) {
      const completed = goal.subTasks.filter((task) => task.status === "Completed").length;
      const remaining = goal.subTasks.length - completed;
      if (seriesIndex === 0) {
        return `<div class="tooltip text-center px-2 py-1 bg-white border rounded shadow">
          <strong>${completed} complete</strong>
        </div>`;
      } else if (goal.subTasks.length === 0) {
        return `<div class="tooltip text-center px-2 py-1 bg-white border rounded shadow">
          <strong>No subtasks</strong>
        </div>`;
      }
      else {
        return `<div class="tooltip text-center px-2 py-1 bg-white border rounded shadow">
          <strong>${remaining} remaining</strong>
        </div>`;
      }
    },
  },
};

  // Initialize the chart after DOM renders
  onMount(async () => {
    const ApexCharts = (await import("apexcharts")).default;

chart = new ApexCharts(document.getElementById(`chart-${goal.id}`), chartOptions);
chart.render();
  });

  // Update chart when percentage changes
  $: if (chart) {
    chart.updateSeries([percentageCompleted, 100 - percentageCompleted]);
  }

  function formatDateLocal(dateString: string | Date | undefined): string {
  if (!dateString) return "No Date Provided";

  const date = new Date(dateString);
  return date.toLocaleDateString("en-US", {
    month: "short",   
    day: "numeric",  
    year: "numeric", 
  });
}
async function handleSubtaskUpdated(updatedSubtask: SubTask) {
    const subtaskIndex = goal.subTasks.findIndex((st: SubTask) => st.id === updatedSubtask.id);
  if (subtaskIndex !== -1) {
    // Update the specific subtask in the array
    goal.subTasks[subtaskIndex] = {
      ...goal.subTasks[subtaskIndex],
      ...updatedSubtask,
    };

    // Assign a new reference to `goal.subTasks` to trigger reactivity
    goal.subTasks = [...goal.subTasks];

    // Perform the API call
    const result = await updateGoal(goal);

    if (!result.isSuccess) {
      console.error("Failed to update goal:", result.message);
    }
}
}
</script>

<Card.Root
  class="hover:shadow-lg hover:cursor-pointer transition-shadow duration-300 hover:shadow-lg hover:scale-105 transition-transform duration-200 flex flex-col"
  onclick={handleCardClick}
>
  <div class="flex-grow flex">
    <!-- Goal Content -->
    <div class="w-1/2">
      <Card.Header>
        <Card.Title class="absolute">
          {goal.title}
        </Card.Title>
        <Card.Description class="text-sm text-gray-500" style="margin-top:2em;">
          <Tooltip.Root>
            <Tooltip.Trigger>
              <span class="flex gap-1 items-center">
                <Calendar class="h-4 w-4" />
                {goal?.targetEndDate ? formatDateLocal(goal.targetEndDate) : "--"}
              </span>
              </Tooltip.Trigger>
              <Tooltip.Content>
                <p class="bg-background p-2">Target Date</p>
                </Tooltip.Content> 
          </Tooltip.Root>          
        </Card.Description>
      </Card.Header>
      <Card.Content class="px-6 pt-2 pb-6">
        <div class="grid w-full items-center gap-4">
          {#if goal.subTasks.length > 0}
          <Label class="text-sm text-gray-500">{goal.subTasks.filter(st => st.status === "Completed").length}/{goal.subTasks.length} Complete</Label>
          <ScrollArea
          class="w-full whitespace-nowrap rounded-md border"
          orientation="horizontal"
        >
          <div class="flex w-max space-x-4">
            {#each goal.subTasks as subTask (subTask.id)}
              <SubtaskMiniCard subTask={subTask} onEdit={handleSubtaskUpdated} />
            {/each}
          </div>
        </ScrollArea>
          {:else}
            <p class="text-sm text-gray-500">No subtasks</p>
          {/if}
        </div>
      </Card.Content>
    </div>
    <!-- Chart Section -->
    <div class="w-1/2 flex items-center justify-center">
      <div id={`chart-${goal.id}`} class="w-full mr-2"></div>
    </div>
  </div>
  <Card.Footer class="flex justify-between mt-auto gap-3">
    {#if goal.subTasks.filter(st => st?.estimatedHours > 0).length > 0}
    <Tooltip.Root>
      <Tooltip.Trigger>
        <div class="flex flex-row gap-1 items-center">
      <Clock />
        <span class="text-sm text-muted-foreground">
          {goal.subTasks.reduce((acc, st) => acc + (st?.status === "Completed" ? st.estimatedHours : 0), 0)} / {goal.subTasks.reduce((acc, st) => acc + st.estimatedHours, 0)}
        </span>
      </div>
      </Tooltip.Trigger>
      <Tooltip.Content>
        <p>Total Estimated Hours</p>
      </Tooltip.Content>
    </Tooltip.Root>
    {/if}
    <div class="flex flex-1"></div>
    <EditGoalDialog {goal} onSave={(updated) => handleGoalUpdated(updated.id, updated.title, updated.status, updated.targetEndDate)}>
      <TooltipIconButton tooltipText="Edit Goal">
        <Pencil class="h-4 w-4 mr-2" />
        Edit
      </TooltipIconButton>
    </EditGoalDialog>
    <TooltipIconButton tooltipText="Delete Goal" variant="destructive" on:click={handleDelete}>
      <Trash class="h-4 w-4 mr-2" />
      Delete
    </TooltipIconButton>
  </Card.Footer>
</Card.Root>
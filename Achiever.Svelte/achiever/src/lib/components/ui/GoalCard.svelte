<script lang="ts">
  import { Button } from "$lib/components/ui/button/index.js";
  import * as Card from "$lib/components/ui/card/index.js";
  import { Label } from "$lib/components/ui/label/index.js";
  import { goto } from "$app/navigation";
  import TooltipIconButton from "./TooltipIconButton.svelte";
  import EditGoalDialog from "./EditGoalDialog.svelte";
  import Pencil from "lucide-svelte/icons/pencil";
  import Trash from "lucide-svelte/icons/trash";
  import { updateGoal, type Goal } from "$lib/api/goals";
    import { onMount } from "svelte";

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
  async function handleGoalUpdated(id: string, title: string, status: number) {
    console.log("Goal updated:", { id, title, status });
    goal.title = title;
    goal.status = status;
    const result = await updateGoal(goal);
    if (result.isSuccess) {
      console.log("Goal successfully updated!");
    } else {
      alert(`Failed to update goal: ${result.message}`);
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
  colors: ["#16A34A", "#D1D5DB"],
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
            formatter: ( seriesIndex ) => {
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
</script>

<Card.Root
  class="hover:shadow-lg hover:cursor-pointer transition-shadow duration-300 hover:shadow-lg hover:scale-105 transition-transform duration-200 flex flex-col"
  onclick={handleCardClick}
>
  <div class="flex-grow flex">
    <!-- Goal Content -->
    <div class="w-2/3 p-4">
      <Card.Header>
        <Card.Title>{goal.title}</Card.Title>
        <Card.Description>
          {goal.subTasks.length} Subtasks | Status: {goal.status}
        </Card.Description>
      </Card.Header>
      <Card.Content>
        <div class="grid w-full items-center gap-4">
          {#if goal.subTasks.length > 0}
            <div class="max-h-36 overflow-y">
              <Label>Subtasks</Label>
              <ul class="list-disc ml-4">
                {#each goal.subTasks as subTask}
                  <li>{subTask.title} - {subTask.status}</li>
                {/each}
              </ul>
            </div>
          {:else}
            <p class="text-sm text-gray-500">No subtasks available</p>
          {/if}
        </div>
      </Card.Content>
    </div>
    <!-- Chart Section -->
    <div class="w-1/3 flex items-center justify-center">
      <div id={`chart-${goal.id}`} class="w-full"></div>
    </div>
  </div>
  <Card.Footer class="flex justify-between mt-auto">
    <EditGoalDialog {goal} onSave={(updated) => handleGoalUpdated(updated.id, updated.title, updated.status)}>
      <TooltipIconButton tooltipText="Edit Goal">
        <Pencil class="h-4 w-4" />
      </TooltipIconButton>
    </EditGoalDialog>
    <Button variant="destructive" on:click={handleDelete}>
      <Trash class="mr-2 h-4 w-4" />
      Delete
    </Button>
  </Card.Footer>
</Card.Root>
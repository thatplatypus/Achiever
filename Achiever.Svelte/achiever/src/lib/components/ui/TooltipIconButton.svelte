<script lang="ts">
    import * as Tooltip from "$lib/components/ui/tooltip";
    export let tooltipText: string;
  
    // Event forwarding for click events
    import { createEventDispatcher } from "svelte";
    const dispatch = createEventDispatcher();
  </script>
  
  <Tooltip.Root>
    <Tooltip.Trigger>
      <button
         class="p-2 border rounded bg-background hover:bg-accent hover:text-accent-foreground focus:outline-none transition-colors"
        on:click={(event) => dispatch('click', event)}
      >
        <slot />
      </button>
    </Tooltip.Trigger>
    <Tooltip.Content>
      <p>{tooltipText}</p>
    </Tooltip.Content>
  </Tooltip.Root>
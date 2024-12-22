<script lang="ts">
    import { goto } from "$app/navigation";
    import { auth } from "$lib/stores/auth";
    import { onMount } from "svelte";

    auth.subscribe((value) => {
        console.log(value);
        if(value.isAuthenticated)
            goto("/goals"); 
    });

    onMount(() => {
       goto("/login");
    });
    /*
    onMount(() => {
    if ($auth.isAuthenticated || $auth.user?.email) {
        goto("/goals");
    } else {
        goto("/login");
    }
    
});
*/
</script>

<script lang="ts">
  import auth from "../data-access/auth-service";
  import { isAuthenticated } from "../data-access/store";
  import { onMount } from "svelte";

  onMount(async () => {
    isAuthenticated.subscribe(authed => {
      if(authed) return;

      auth.login();
    });
  });  
</script>


{#if $isAuthenticated}
  <slot></slot>
{/if}

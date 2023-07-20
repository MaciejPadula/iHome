<script lang="ts">
  import { ListPlaceholder } from "flowbite-svelte";
  import { loading } from "../data-access/store";
</script>

{#if $loading === true}
  <ListPlaceholder class="w-full" />
{/if}

{#if $loading === false}
  <slot></slot>
{/if}
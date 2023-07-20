<script lang="ts">
  import { onMount } from 'svelte';

  export let data;

  onMount(async () => {
    console.log(data.id);
  });
</script>

<div>
  {data.id}
</div>
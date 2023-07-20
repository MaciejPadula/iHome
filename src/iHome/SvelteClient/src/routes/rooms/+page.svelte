<script lang="ts">
  import { rooms } from "../../data-access/store";
  import Room from "../../components/Room.svelte";
  import { onMount } from "svelte";
  import { getRooms } from "../../data-access/api/rooms-service";

  onMount(async () => {
    const result = await getRooms();
    rooms.set(result);
  });

  const roomUrl = (id: string) => `rooms/${id}`;
</script>

{#each $rooms as room}
  <Room bind:room={room} urlGenerator={roomUrl} />
{/each}
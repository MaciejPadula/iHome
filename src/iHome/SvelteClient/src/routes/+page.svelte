<script lang="ts">
  import { onMount } from "svelte";
  import auth from "../data-access/auth-service";
  import { user } from "../data-access/store";

	onMount(async () => {
		await auth.loadUser();
	});

	async function login() {
		await auth.loginWithRedirect();
		await auth.loadUser();
	}

	user.subscribe(us => console.log(us));
</script>

<svelte:head>
	<title>Home</title>
	<meta name="description" content="iHome" />
</svelte:head>

<section>
	<button on:click={login}>Login</button>
</section>

<style>

</style>

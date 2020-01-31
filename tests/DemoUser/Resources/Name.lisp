(defblock :name curl :is-top t
	(worker
		:worker-name name
		:verb "name"
		:description "Sends message to a named subscriber."
		:usage-samples (
			"name bob"))

	(some-text
		:classes term
		:alias name
		:action argument
		:description "Name"
		:doc-subst "name")

	(end)
)

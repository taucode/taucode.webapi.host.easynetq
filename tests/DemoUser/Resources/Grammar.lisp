(defblock :name curl :is-top t
	(worker
		:description "Sends message to a named subscriber."
		:usage-samples (
			"-to andy 'Hello andy!'"))

	(multi-text
		:classes key
		:values "-to"
		:alias to
		:action key)

	(some-text
		:classes term
		:alias recipient-name
		:action value
		:description "Recipient name"
		:doc-subst "recipient name")

	(some-text
		:classes string
		:alias message-text
		:action argument
		:description "Message text"
		:doc-subst "message text")

	(end)
)
